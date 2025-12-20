using AutoMapper;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Enums;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Contracts;

namespace StuffyHelper.Authorization.Contracts.AutoMapper;

/// <summary>
/// User auto mapper profile
/// </summary>
public class UserAutoMapperProfile : Profile
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public UserAutoMapperProfile()
    {
        CreateMap<StuffyClaims, UserShortEntry>()
            .ForMember(usr => usr.Id, opt => opt.MapFrom(sc => sc.UserId))
            .ForMember(usr => usr.Name, opt => opt.MapFrom(sc => sc.Username));
        CreateMap<GetUserEntry, UserShortEntry>();

        CreateMap<StuffyUser, GetUserEntry>();
        CreateMap<StuffyUser, UserShortEntry>()
            .ForMember(use => use.Name, opt => opt.MapFrom(su => su.UserName));

        CreateMap<(StuffyUser UserEntry, IList<string>? Roles), GetUserEntry>()
            .ForMember(ue => ue.Id, opt => opt.MapFrom(src => src.UserEntry.Id))
            .ForMember(ue => ue.Name, opt => opt.MapFrom(src => src.UserEntry.UserName))
            .ForMember(ue => ue.Email, opt => opt.MapFrom(src => src.UserEntry.Email))
            .ForMember(ue => ue.FirstName, opt => opt.MapFrom(src => src.UserEntry.FirstName))
            .ForMember(ue => ue.MiddleName, opt => opt.MapFrom(src => src.UserEntry.MiddleName))
            .ForMember(ue => ue.LastName, opt => opt.MapFrom(src => src.UserEntry.LastName))
            .ForMember(ue => ue.Phone, opt => opt.MapFrom(src => src.UserEntry.PhoneNumber))
            .ForMember(ue => ue.ImageUri, opt => opt.MapFrom(src => src.UserEntry.ImageUri))
            .ForMember(ue => ue.Role, opt => opt.MapFrom(src =>
                src.Roles != null && src.Roles.Any() && src.Roles.Contains(nameof(UserType.Admin)) == true
                    ? nameof(UserType.Admin)
                    : nameof(UserType.User)));
    }
}