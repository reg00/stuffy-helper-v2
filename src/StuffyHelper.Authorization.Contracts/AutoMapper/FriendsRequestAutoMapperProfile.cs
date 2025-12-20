using AutoMapper;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Authorization.Contracts.AutoMapper;

/// <summary>
/// Friends request auto mapper profile
/// </summary>
public class FriendsRequestAutoMapperProfile : Profile
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public FriendsRequestAutoMapperProfile()
    {
        CreateMap<FriendsRequest, FriendsRequestShort>()
            .ForMember(frs => frs.UserNameFrom, opt => opt.MapFrom(fr => fr.UserFrom.UserName))
            .ForMember(frs => frs.UserNameTo, opt => opt.MapFrom(fr => fr.UserTo.UserName));

        CreateMap<(string IncomingUserId, string friendId), FriendsRequest>()
            .ForMember(fr => fr.UserIdFrom, opt =>
            {
                opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.IncomingUserId));
                opt.MapFrom(src => src.IncomingUserId);
            })
            .ForMember(fr => fr.UserIdTo, opt =>
            {
                opt.PreCondition(src => !string.IsNullOrWhiteSpace(src.friendId));
                opt.MapFrom(src => src.friendId);
            });
    }
}