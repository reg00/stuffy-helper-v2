using AutoMapper;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.AutoMapper;

/// <summary>
/// Checkout models auto mapper
/// </summary>
public class CheckoutAutoMapperProfile : Profile
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public CheckoutAutoMapperProfile()
    {
        CreateMap<Guid, CheckoutEntry>()
            .ForMember(ce => ce.EventId, opt => opt.MapFrom(eventId => eventId))
            .ForMember(ce => ce.CreatedDate, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}