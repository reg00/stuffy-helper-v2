using AutoMapper;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.AutoMapper;

/// <summary>
/// Purchase auto mapper
/// </summary>
public class PurchaseAutoMapperProfile : Profile
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public PurchaseAutoMapperProfile()
    {
        CreateMap<AddPurchaseEntry, PurchaseEntry>()
            .ForMember(pe => pe.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
        CreateMap<PurchaseEntry, GetPurchaseEntry>()
            .ForMember(gpe => gpe.EventId, opt => opt.MapFrom(pe => pe.EventId));
        CreateMap<PurchaseEntry, PurchaseShortEntry>()
            .ForMember(gpe => gpe.EventId, opt => opt.MapFrom(pe => pe.EventId));
    }
}