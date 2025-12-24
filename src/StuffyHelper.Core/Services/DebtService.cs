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
    /// <inheritdoc />
    public class DebtService : IDebtService
    {
        private readonly IDebtRepository _debtRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ICheckoutRepository _checkoutRepository;
        private readonly IPurchaseUsageRepository _purchaseUsageRepository;
        private readonly IPurchaseService _purchaseService;
        private readonly IAuthorizationClient _authorizationClient;
        private readonly IMapper _mapper;
        
        /// <summary>
        /// Ctor.
        /// </summary>
        public DebtService(
            IDebtRepository debtRepository,
            IEventRepository eventRepository,
            ICheckoutRepository checkoutRepository,
            IPurchaseUsageRepository purchaseUsageRepository,
            IPurchaseService purchaseService, 
            IAuthorizationClient authorizationClient,
            IMapper mapper)
        {
            _debtRepository = debtRepository;
            _eventRepository = eventRepository;
            _checkoutRepository = checkoutRepository;
            _purchaseUsageRepository = purchaseUsageRepository;
            _purchaseService = purchaseService;
            _authorizationClient = authorizationClient;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<GetDebtEntry> GetDebtAsync(Guid eventId, Guid debtId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(debtId, nameof(debtId));

            var entry = await _debtRepository.GetDebtAsync(eventId, debtId, cancellationToken);
            var lender = await _authorizationClient.GetUserById(entry.LenderId, cancellationToken);
            var debtor = await _authorizationClient.GetUserById(entry.DebtorId, cancellationToken);

            return _mapper.Map<GetDebtEntry>((entry,_mapper.Map<UserShortEntry>(lender), _mapper.Map<UserShortEntry>(debtor)));
        }

        /// <inheritdoc />
        public async Task<Response<GetDebtEntry>> GetDebtsByUserAsync(
            string userId,
            Guid eventId, 
            int offset = 0,
            int limit = 10,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotEmptyOrWhiteSpace(userId, nameof(userId));

            var debts = new List<GetDebtEntry>();
            var resp = await _debtRepository.GetDebtsByUserAsync(userId, eventId, offset, limit, cancellationToken);

            foreach (var dbDebt in resp.Data)
            {
                var lender = await _authorizationClient.GetUserById(dbDebt.LenderId, cancellationToken);
                var debtor = await _authorizationClient.GetUserById(dbDebt.DebtorId, cancellationToken);

                debts.Add(_mapper.Map<GetDebtEntry>((dbDebt, _mapper.Map<UserShortEntry>(lender), _mapper.Map<UserShortEntry>(debtor))));
            }

            return new Response<GetDebtEntry>()
            {
                Data = debts,
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        /// <inheritdoc />
        public async Task<GetDebtEntry> SendDebtAsync(string userId, Guid eventId, Guid debtId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(debtId, nameof(debtId));
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            var debt = await _debtRepository.GetDebtAsync(eventId, debtId, cancellationToken);

            if (debt is null || debt.DebtorId != userId)
                throw new EntityNotFoundException($"Debt Id '{debtId}' not found");

            debt.IsSent = true;

            var result = await _debtRepository.UpdateDebtAsync(debt, cancellationToken);
            var lender = await _authorizationClient.GetUserById(result.LenderId, cancellationToken);
            var debtor = await _authorizationClient.GetUserById(result.DebtorId, cancellationToken);

            return _mapper.Map<GetDebtEntry>((result,_mapper.Map<UserShortEntry>(lender), _mapper.Map<UserShortEntry>(debtor)));
        }

        /// <inheritdoc />
        public async Task<GetDebtEntry> ConfirmDebtAsync(string userId, Guid eventId, Guid debtId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(debtId, nameof(debtId));
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            var debt = await _debtRepository.GetDebtAsync(eventId, debtId, cancellationToken);

            if (debt is null || debt.LenderId != userId)
                throw new EntityNotFoundException($"Debt Id '{debtId}' not found!");

            if (!debt.IsSent)
                throw new BadRequestException("Cannot confirm not sent debt {DebtId}. User: {UserId}", debtId, userId);

            debt.IsComfirmed = true;

            var result = await _debtRepository.UpdateDebtAsync(debt, cancellationToken);
            var lender = await _authorizationClient.GetUserById(result.LenderId, cancellationToken);
            var debtor = await _authorizationClient.GetUserById(result.DebtorId, cancellationToken);

            return _mapper.Map<GetDebtEntry>((result,_mapper.Map<UserShortEntry>(lender), _mapper.Map<UserShortEntry>(debtor)));
        }

        /// <inheritdoc />
        public async Task CheckoutEvent(Guid eventId, string? userId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var debts = new List<DebtEntry>();
            var @event = await _eventRepository.GetEventAsync(eventId, userId, cancellationToken);

            if (@event is null)
            {
                throw new EntityNotFoundException($"Event Id '{eventId}' not found");
            }

            if(@event.Purchases.All(purchase => purchase.IsComplete))
                return;
            
            var checkout = _mapper.Map<CheckoutEntry>(eventId);
            checkout = await _checkoutRepository.AddCheckoutAsync(checkout, cancellationToken);

            foreach (var purchase in @event.Purchases.Where(x => !x.IsComplete).ToList())
            {
                var fullAmount = purchase.PurchaseUsages.Select(x => x.Amount).Sum();
                
                foreach (var usage in purchase.PurchaseUsages)
                {
                    // Сам себе не должен
                    if (purchase.Owner.UserId == usage.Participant.UserId)
                        continue;

                    var debt = debts.FirstOrDefault(x => x.LenderId == purchase.Owner.UserId && x.DebtorId == usage.Participant.UserId ||
                                                         x.DebtorId == purchase.Owner.UserId && x.LenderId == usage.Participant.UserId);

                    var userDebt = purchase.Cost * usage.Amount / fullAmount;
                    if (debt != null)
                    {
                        debt.Amount += debt.LenderId == purchase.Owner.UserId ? userDebt : -userDebt;
                    }
                    else
                    {
                        debts.Add(new DebtEntry()
                        {
                            //TODO: переделать долги
                            //Amount = purchase.IsPartial ? purchase.Cost * usage.Amount : (purchase.Amount * purchase.Cost) / purchase.PurchaseUsages.Count,
                            Amount = userDebt,
                            LenderId = purchase.Owner.UserId,
                            DebtorId = usage.Participant.UserId,
                            CheckoutId = checkout.Id,
                            EventId = eventId,
                        });
                    }

                    usage.CheckoutId = checkout.Id;

                    await _purchaseUsageRepository.UpdatePurchaseUsageAsync(usage, cancellationToken);
                }

                await _purchaseService.CompletePurchaseAsync(eventId, purchase.Id, cancellationToken);
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
                }

                var existsDebt = await _debtRepository.GetDebtAsync(debt.LenderId, debt.DebtorId, debt.EventId, cancellationToken);

                if (existsDebt != null)
                {
                    existsDebt.Amount += debt.Amount;
                    existsDebt.IsComfirmed = false;
                    existsDebt.IsSent = false;
                    existsDebt.CheckoutId = debt.CheckoutId;

                    await _debtRepository.UpdateDebtAsync(existsDebt, cancellationToken);
                }
                else
                {
                    await _debtRepository.AddDebtAsync(debt, cancellationToken);
                }
            }
        }
    }
}
