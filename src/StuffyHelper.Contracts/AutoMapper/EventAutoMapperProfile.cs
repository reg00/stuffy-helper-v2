using AutoMapper;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.AutoMapper;

/// <summary>
/// Event auto mapper profile
/// </summary>
public class EventAutoMapperProfile : Profile
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public EventAutoMapperProfile()
    {
        CreateMap<(EventEntry Event, UserShortEntry User, List<ParticipantShortEntry> Participants), GetEventEntry>()
            .ForMember(gee => gee.Id, opt => opt.MapFrom(src => src.Event.Id))
            .ForMember(gee => gee.Name, opt => opt.MapFrom(src => src.Event.Name))
            .ForMember(gee => gee.Description, opt => opt.MapFrom(src => src.Event.Description))
            .ForMember(gee => gee.CreatedDate, opt => opt.MapFrom(src => src.Event.CreatedDate))
            .ForMember(gee => gee.IsCompleted, opt => opt.MapFrom(src => src.Event.IsCompleted))
            .ForMember(gee => gee.EventDateEnd, opt => opt.MapFrom(src => src.Event.EventDateEnd))
            .ForMember(gee => gee.EventDateStart, opt => opt.MapFrom(src => src.Event.EventDateStart))
            .ForMember(gee => gee.MediaUri, opt => opt.MapFrom(src => src.Event.ImageUri))
            .ForMember(gee => gee.Purchases, opt => opt.MapFrom(src => src.Event.Purchases))
            .ForMember(gee => gee.Medias, opt => opt.MapFrom(src => src.Event.Medias))
            .ForMember(gee => gee.User, opt => opt.MapFrom(src => src.User))
            .ForMember(gee => gee.Participants, opt => opt.MapFrom(src => src.Participants));
        CreateMap<EventEntry, EventShortEntry>();
        CreateMap<(AddEventEntry Event, GetUserEntry User), EventEntry>()
            .ForMember(ee => ee.Name, opt => opt.MapFrom(src => src.Event.Name))
            .ForMember(ee => ee.Description, opt => opt.MapFrom(src => src.Event.Description))
            .ForMember(ee => ee.EventDateStart, opt => opt.MapFrom(src => src.Event.EventDateStart))
            .ForMember(ee => ee.EventDateEnd, opt => opt.MapFrom(src => src.Event.EventDateEnd))
            .ForMember(ee => ee.EventDateEnd, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(ee => ee.UserId, opt => opt.MapFrom(src => src.User.Id))
            .ForMember(ee => ee.IsCompleted, opt => opt.MapFrom(src => false))
            .ForMember(ee => ee.IsActive, opt => opt.MapFrom(src => true));
    }
}