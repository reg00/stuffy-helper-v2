using AutoMapper;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.AutoMapper;

/// <summary>
/// Purchase tag auto mapper
/// </summary>
public class PurchaseTagAutoMapperProfile : Profile
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public PurchaseTagAutoMapperProfile()
    {
        CreateMap<PurchaseTagEntry, GetPurchaseTagEntry>();
        CreateMap<PurchaseTagEntry, PurchaseTagShortEntry>();
        CreateMap<UpsertPurchaseTagEntry, PurchaseTagEntry>()
            .ForMember(pte => pte.IsActive, opt => opt.MapFrom(_ => true));
    }
}