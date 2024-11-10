using AutoMapper;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Enums;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Authorization.Core.AutoMapper;

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
        CreateMap<StuffyUser, UserShortEntry>()
            .ForMember(use => use.Name, opt => opt.MapFrom(su => su.UserName));

        CreateMap<(StuffyUser UserEntry, IList<string>? Roles), GetUserEntry>()
            .ForMember(ue => ue.Role, opt => opt.MapFrom(src =>
               src.Roles != null && src.Roles.Any() && src.Roles.Contains(nameof(UserType.Admin)) == true ? nameof(UserType.Admin) : nameof(UserType.User)));
    }
}