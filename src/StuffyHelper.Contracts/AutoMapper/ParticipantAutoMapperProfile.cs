using AutoMapper;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.AutoMapper;

/// <summary>
/// Participant auto mapper
/// </summary>
public class ParticipantAutoMapperProfile : Profile
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public ParticipantAutoMapperProfile()
    {
        CreateMap<UpsertParticipantEntry, ParticipantEntry>();
        CreateMap<(ParticipantEntry Participant, GetUserEntry User), GetParticipantEntry>()
            .ForMember(gpe => gpe.Id, opt => opt.MapFrom(src => src.Participant.Id))
            .ForMember(gpe => gpe.Event, opt => opt.MapFrom(src => src.Participant.Event))
            .ForMember(gpe => gpe.Purchases, opt => opt.MapFrom(src => src.Participant.Purchases))
            .ForMember(gpe => gpe.User, opt => opt.MapFrom(src => src.User))
            .ForMember(gpe => gpe.PurchaseUsages, opt => opt.MapFrom(src => src.Participant.PurchaseUsages));
        
        CreateMap<(ParticipantEntry Participant, UserShortEntry User), ParticipantShortEntry>()
            .ForMember(gse => gse.Id, opt => opt.MapFrom(src => src.Participant.Id))
            .ForMember(gse => gse.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(gse => gse.ImageUri, opt => opt.MapFrom(src => src.User.ImageUri));
    }
}