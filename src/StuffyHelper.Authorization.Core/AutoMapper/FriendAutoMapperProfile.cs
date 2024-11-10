using AutoMapper;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Authorization.Core.AutoMapper;

/// <summary>
/// Friends auto mapper profile
/// </summary>
public class FriendAutoMapperProfile : Profile
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public FriendAutoMapperProfile()
    {
        CreateMap<FriendEntry, FriendShortEntry>()
            .ForMember(fse => fse.UserName, opt => opt.MapFrom(fe => fe.User.UserName))
            .ForMember(fse => fse.FriendName, opt => opt.MapFrom(fe => fe.Friend.UserName));
    }
}