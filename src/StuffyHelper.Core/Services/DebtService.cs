using AutoMapper;
using EnsureThat;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;
using StuffyHelper.Data.Repository.Interfaces;
using EntityNotFoundException = Reg00.Infrastructure.Errors.EntityNotFoundException;

namespace StuffyHelper.Core.Services
{
    public class DebtService : IDebtService
    {
        private readonly IDebtRepository _debtStore;
        private readonly IEventRepository _eventStore;
        private readonly ICheckoutRepository _checkoutStore;
        private readonly IPurchaseUsageRepository _purchaseUsageStore;
        private readonly IPurchaseService _purchaseService;
        private readonly IAuthorizationClient _authorizationClient;
        private readonly IMapper _mapper;

        public DebtService(
            IDebtRepository debtStore,
            IEventRepository eventStore,
            ICheckoutRepository checkoutStore,
            IPurchaseUsageRepository purchaseUsageStore,
            IPurchaseService purchaseService, 
            IAuthorizationClient authorizationClient,
            IMapper mapper)
        {
            _debtStore = debtStore;
            _eventStore = eventStore;
            _checkoutStore = checkoutStore;
            _purchaseUsageStore = purchaseUsageStore;
            _purchaseService = purchaseService;
            _authorizationClient = authorizationClient;
            _mapper = mapper;
        }

        public async Task<GetDebtEntry> GetDebtAsync(Guid debtId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(debtId, nameof(debtId));

            var entry = await _debtStore.GetDebtAsync(debtId, cancellationToken);
            var lender = await _authorizationClient.GetUserById(entry.LenderId, cancellationToken);
            var debtor = await _authorizationClient.GetUserById(entry.DebtorId, cancellationToken);

            return _mapper.Map<GetDebtEntry>((entry,_mapper.Map<UserShortEntry>(lender), _mapper.Map<UserShortEntry>(debtor)));
        }

        public async Task<Response<GetDebtEntry>> GetDebtsAsync(
            int offset = 0,
            int limit = 10,
            string? lenderId = null,
            string? debtorId = null,
            bool? isSent = null,
            bool? isConfirmed = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _debtStore.GetDebtsAsync(offset, limit, lenderId, debtorId, isSent, isConfirmed, cancellationToken);

            var debts = new List<GetDebtEntry>();

            foreach (var debt in resp.Data)
            {
                var lender = await _authorizationClient.GetUserById(debt.LenderId, cancellationToken);
                var debtor = await _authorizationClient.GetUserById(debt.DebtorId, cancellationToken);

                debts.Add(_mapper.Map<GetDebtEntry>((debt,_mapper.Map<UserShortEntry>(lender), _mapper.Map<UserShortEntry>(debtor))));
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
                var lender = await _authorizationClient.GetUserById(dbDebt.LenderId, cancellationToken);
                var debtor = await _authorizationClient.GetUserById(dbDebt.DebtorId, cancellationToken);

                debts.Add(_mapper.Map<GetDebtEntry>((dbDebt,_mapper.Map<UserShortEntry>(lender), _mapper.Map<UserShortEntry>(debtor))));

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
        //    var lender = await _authorizationService.GetUser(userId: result.LenderId);
        //    var debtor = await _authorizationService.GetUser(userId: result.DebtorId);

        //    return new GetDebtEntry(result, lender, debtor);
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

            if (debt is null || debt.DebtorId != userId)
                throw new EntityNotFoundException($"Debt Id '{debtId}' not found");

            debt.IsSent = true;

            var result = await _debtStore.UpdateDebtAsync(debt, cancellationToken);
            var lender = await _authorizationClient.GetUserById(result.LenderId, cancellationToken);
            var debtor = await _authorizationClient.GetUserById(result.DebtorId, cancellationToken);

            return _mapper.Map<GetDebtEntry>((result,_mapper.Map<UserShortEntry>(lender), _mapper.Map<UserShortEntry>(debtor)));
        }

        public async Task<GetDebtEntry> ConfirmDebtAsync(string userId, Guid debtId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(debtId, nameof(debtId));
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            var debt = await _debtStore.GetDebtAsync(debtId, cancellationToken);

            if (debt is null || debt.LenderId != userId)
                throw new EntityNotFoundException($"Debt Id '{debtId}' not found!");

            if (!debt.IsSent)
                throw new BadRequestException("Cannot confirm not sented debt");

            debt.IsComfirmed = true;

            var result = await _debtStore.UpdateDebtAsync(debt, cancellationToken);
            var lender = await _authorizationClient.GetUserById(result.LenderId, cancellationToken);
            var debtor = await _authorizationClient.GetUserById(result.DebtorId, cancellationToken);

            return _mapper.Map<GetDebtEntry>((result,_mapper.Map<UserShortEntry>(lender), _mapper.Map<UserShortEntry>(debtor)));
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

            var checkout = _mapper.Map<CheckoutEntry>(eventId);
            checkout = await _checkoutStore.AddCheckoutAsync(checkout, cancellationToken);

            foreach (var purchase in @event.Purchases.Where(x => x.IsComplete == false))
            {
                foreach (var usage in purchase.PurchaseUsages)
                {
                    // Сам себе не должен
                    if (purchase.Owner.UserId == usage.Participant.UserId)
                        continue;

                    var debt = debts.FirstOrDefault(x => x.LenderId == purchase.Owner.UserId && x.DebtorId == usage.Participant.UserId ||
                                                         x.DebtorId == purchase.Owner.UserId && x.LenderId == usage.Participant.UserId);

                    if (debt != null)
                    {
                        debt.Amount += debt.LenderId == purchase.Owner.UserId ? usage.Amount : -usage.Amount;
                    }
                    else
                    {
                        debts.Add(new DebtEntry()
                        {
                            Amount = purchase.IsPartial ? purchase.Cost * usage.Amount : (purchase.Amount * purchase.Cost) / purchase.PurchaseUsages.Count,
                            LenderId = purchase.Owner.UserId,
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
                    (debt.LenderId, debt.DebtorId) = (debt.DebtorId, debt.LenderId);
                    //var temp = debt.LenderId;
                    //debt.LenderId = debt.DebtorId;
                    //debt.DebtorId = temp;
                }

                var existsDebt = await _debtStore.GetDebtAsync(debt.LenderId, debt.DebtorId, debt.EventId, cancellationToken);

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
