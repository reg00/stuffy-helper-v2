using AutoMapper;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.AutoMapper;

/// <summary>
/// Debt auto mapper
/// </summary>
public class DebtAutoMapperProfile : Profile
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public DebtAutoMapperProfile()
    {
        CreateMap<(GetDebtEntry Debt, UserShortEntry Lender, UserShortEntry Debtor), GetDebtEntry>()
            .ForMember(gbe => gbe.Id, opt => opt.MapFrom(src => src.Debt.Id))
            .ForMember(gbe => gbe.Amount, opt => opt.MapFrom(src => src.Debt.Amount))
            .ForMember(gbe => gbe.IsSent, opt => opt.MapFrom(src => src.Debt.IsSent))
            .ForMember(gbe => gbe.IsComfirmed, opt => opt.MapFrom(src => src.Debt.IsComfirmed))
            .ForMember(gbe => gbe.Event, opt => opt.MapFrom(src => src.Debt.Event))
            .ForMember(gbe => gbe.Lender, opt => opt.MapFrom(src => src.Lender))
            .ForMember(gbe => gbe.Debtor, opt => opt.MapFrom(src => src.Debtor));
    }
}