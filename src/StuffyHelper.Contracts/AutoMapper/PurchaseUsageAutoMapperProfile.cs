using AutoMapper;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.AutoMapper;

/// <summary>
/// Purchase usage auto mapper
/// </summary>
public class PurchaseUsageAutoMapperProfile : Profile
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public PurchaseUsageAutoMapperProfile()
    {
        CreateMap<UpsertPurchaseUsageEntry, PurchaseUsageEntry>();
        CreateMap<PurchaseUsageEntry, PurchaseUsageShortEntry>()
            .ForMember(puse => puse.PurchaseUsageId, opt => opt.MapFrom(pue => pue.Id));

        CreateMap<(PurchaseUsageEntry PurchaseUsage, ParticipantShortEntry Participant), GetPurchaseUsageEntry>()
            .ForMember(gpue => gpue.Id, opt => opt.MapFrom(src => src.PurchaseUsage.Id))
            .ForMember(gpue => gpue.Amount, opt => opt.MapFrom(src => src.PurchaseUsage.Amount))
            .ForMember(gpue => gpue.Purchase, opt => opt.MapFrom(src => src.PurchaseUsage.Purchase))
            .ForMember(gpue => gpue.Participant, opt => opt.MapFrom(src => src.Participant));
    }
}