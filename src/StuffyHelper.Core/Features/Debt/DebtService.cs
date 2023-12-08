﻿using EnsureThat;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Checkout;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.Core.Features.PurchaseUsage;

namespace StuffyHelper.Core.Features.Debt
{
    public class DebtService : IDebtService
    {
        private readonly IDebtStore _debtStore;
        private readonly IEventStore _eventStore;
        private readonly ICheckoutStore _checkoutStore;
        private readonly IPurchaseUsageStore _purchaseUsageStore;
        private readonly IAuthorizationService _authorizationService;
        private readonly IPurchaseService _purchaseService;

        public DebtService(
            IDebtStore debtStore,
            IEventStore eventStore,
            ICheckoutStore checkoutStore,
            IPurchaseUsageStore purchaseUsageStore,
            IAuthorizationService authorizationService,
            IPurchaseService purchaseService)
        {
            _debtStore = debtStore;
            _eventStore = eventStore;
            _checkoutStore = checkoutStore;
            _purchaseUsageStore = purchaseUsageStore;
            _authorizationService = authorizationService;
            _purchaseService = purchaseService;
        }

        public async Task<GetDebtEntry> GetDebtAsync(Guid debtId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(debtId, nameof(debtId));

            var entry = await _debtStore.GetDebtAsync(debtId, cancellationToken);
            var borrower = await _authorizationService.GetUserById(entry.BorrowerId);
            var debtor = await _authorizationService.GetUserById(entry.DebtorId);

            return new GetDebtEntry(entry, borrower, debtor);
        }

        public async Task<Response<GetDebtEntry>> GetDebtsAsync(
            int offset = 0,
            int limit = 10,
            string? borrowerId = null,
            string? debtorId = null,
            bool? isSent = null,
            bool? isConfirmed = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _debtStore.GetDebtsAsync(offset, limit, borrowerId, debtorId, isSent, isConfirmed, cancellationToken);

            var debts = new List<GetDebtEntry>();

            foreach (var debt in resp.Data)
            {
                var borrower = await _authorizationService.GetUserById(debt.BorrowerId);
                var debtor = await _authorizationService.GetUserById(debt.DebtorId);

                debts.Add(new GetDebtEntry(debt, borrower, debtor));
            }

            return new Response<GetDebtEntry>()
            {
                Data = debts,
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<Response<GetDebtEntry>> GetDebtsByUserAsync(
            string userId,
            int offset = 0,
            int limit = 10,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotEmptyOrWhiteSpace(userId, nameof(userId));

            var debts = new List<GetDebtEntry>();
            var resp = await _debtStore.GetDebtsByUserAsync(userId, offset, limit, cancellationToken);

            foreach (var dbDebt in resp.Data)
            {
                var borrower = await _authorizationService.GetUserById(dbDebt.BorrowerId);
                var debtor = await _authorizationService.GetUserById(dbDebt.DebtorId);

                debts.Add(new GetDebtEntry(dbDebt, borrower, debtor));
            }

            return new Response<GetDebtEntry>()
            {
                Data = debts,
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        //public async Task<GetDebtEntry> AddDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default)
        //{
        //    EnsureArg.IsNotNull(debt, nameof(debt));

        //    var result = await _debtStore.AddDebtAsync(debt, cancellationToken);
        //    var borrower = await _authorizationService.GetUser(userId: result.BorrowerId);
        //    var debtor = await _authorizationService.GetUser(userId: result.DebtorId);

        //    return new GetDebtEntry(result, borrower, debtor);
        //}

        //public async Task DeleteDebtAsync(Guid debtId, CancellationToken cancellationToken = default)
        //{
        //    EnsureArg.IsNotDefault(debtId, nameof(debtId));

        //    await _debtStore.DeleteDebtAsync(debtId, cancellationToken);
        //}

        public async Task<GetDebtEntry> SendDebtAsync(string userId, Guid debtId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(debtId, nameof(debtId));
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            var debt = await _debtStore.GetDebtAsync(debtId, cancellationToken);

            if (debt is null || debt.BorrowerId != userId)
                throw new EntityNotFoundException($"Debt Id '{debtId}' not found");

            debt.IsSent = true;

            var result = await _debtStore.UpdateDebtAsync(debt, cancellationToken);
            var borrower = await _authorizationService.GetUserById(result.BorrowerId);
            var debtor = await _authorizationService.GetUserById(result.DebtorId);

            return new GetDebtEntry(result, borrower, debtor);
        }

        public async Task<GetDebtEntry> ConfirmDebtAsync(string userId, Guid debtId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(debtId, nameof(debtId));
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            var debt = await _debtStore.GetDebtAsync(debtId, cancellationToken);

            if (debt is null || debt.DebtorId != userId)
                throw new EntityNotFoundException($"Debt Id '{debtId}' not found!");

            if (!debt.IsSent)
                throw new StuffyException("Cannot confirm not sented debt");

            debt.IsComfirmed = true;

            var result = await _debtStore.UpdateDebtAsync(debt, cancellationToken);
            var borrower = await _authorizationService.GetUserById(result.BorrowerId);
            var debtor = await _authorizationService.GetUserById(result.DebtorId);

            return new GetDebtEntry(result, borrower, debtor);
        }

        public async Task CheckoutEvent(Guid eventId, string? userId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var debts = new List<DebtEntry>();
            var @event = await _eventStore.GetEventAsync(eventId, userId, cancellationToken);

            if (@event is null)
            {
                throw new EntityNotFoundException($"Event Id '{eventId}' not found");
            }

            var checkout = await _checkoutStore.AddCheckoutAsync(new CheckoutEntry(eventId), cancellationToken);

            foreach (var purchase in @event.Purchases.Where(x => x.IsComplete == false))
            {
                foreach (var usage in purchase.PurchaseUsages)
                {
                    // Сам себе не должен
                    if (purchase.Owner.UserId == usage.Participant.UserId)
                        continue;

                    var debt = debts.FirstOrDefault(x => x.BorrowerId == purchase.Owner.UserId && x.DebtorId == usage.Participant.UserId ||
                                                         x.DebtorId == purchase.Owner.UserId && x.BorrowerId == usage.Participant.UserId);

                    if (debt != null)
                    {
                        debt.Amount += debt.BorrowerId == purchase.Owner.UserId ? usage.Amount : -usage.Amount;
                    }
                    else
                    {
                        debts.Add(new DebtEntry()
                        {
                            Amount = purchase.IsPartial ? purchase.Cost * usage.Amount : (purchase.Amount * purchase.Cost) / purchase.PurchaseUsages.Count,
                            BorrowerId = purchase.Owner.UserId,
                            DebtorId = usage.Participant.UserId,
                            CheckoutId = checkout.Id,
                            EventId = eventId,
                        });
                    }

                    usage.CheckoutId = checkout.Id;

                    await _purchaseUsageStore.UpdatePurchaseUsageAsync(usage, cancellationToken);
                }

                await _purchaseService.CompletePurchaseAsync(purchase.Id, cancellationToken);
            }

            foreach (var debt in debts)
            {
                // Взаимные долги списываем
                if (debt.Amount == 0)
                    continue;

                // Если сумма долга < 0, меняем местами должника и заемщика
                if (debt.Amount < 0)
                {
                    debt.Amount *= -1;
                    (debt.BorrowerId, debt.DebtorId) = (debt.DebtorId, debt.BorrowerId);
                    //var temp = debt.BorrowerId;
                    //debt.BorrowerId = debt.DebtorId;
                    //debt.DebtorId = temp;
                }

                var existsDebt = await _debtStore.GetDebtAsync(debt.BorrowerId, debt.DebtorId, debt.EventId, cancellationToken);

                if (existsDebt != null)
                {
                    existsDebt.Amount += debt.Amount;
                    existsDebt.IsComfirmed = false;
                    existsDebt.IsSent = false;
                    existsDebt.CheckoutId = debt.CheckoutId;

                    await _debtStore.UpdateDebtAsync(existsDebt, cancellationToken);
                }
                else
                {
                    await _debtStore.AddDebtAsync(debt, cancellationToken);
                }
            }
        }
    }
}
