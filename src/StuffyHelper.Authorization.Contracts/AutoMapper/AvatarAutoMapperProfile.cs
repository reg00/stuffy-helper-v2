using AutoMapper;
using Microsoft.AspNetCore.Http;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Minio.Features.Common;
using StuffyHelper.Minio.Features.Helpers;

namespace StuffyHelper.Authorization.Contracts.AutoMapper;

/// <summary>
/// Avatar auto mapper profile
/// </summary>
public class AvatarAutoMapperProfile : Profile
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public AvatarAutoMapperProfile()
    {
        CreateMap<(Stream? Stream, AvatarEntry Avatar), MediaBlobEntry>()
            .ForMember(mbe => mbe.ContentType, opt => opt.MapFrom(src => FileTypeMapper.MapContentTypeFromFileType(src.Avatar.FileType)))
            .ForMember(mbe => mbe.Ext, opt => opt.MapFrom(src => FileTypeMapper.MapExtFromFileType(src.Avatar.FileType)))
            .ForMember(mbe => mbe.FileName, opt => opt.MapFrom(src => src.Avatar.FileName))
            .AfterMap((src, dest) =>
            {
                if(src.Stream == null)
                    return;
                
                src.Stream.Seek(0, SeekOrigin.Begin);
                dest.Stream = src.Stream;
            });

        CreateMap<AddAvatarEntry, AvatarEntry>()
            .ForMember(ae => ae.FileName, opt =>
            {
                opt.PreCondition(aae => aae.File != null);
                opt.MapFrom(aae => Path.GetFileNameWithoutExtension(aae.File!.FileName));
            })
            .ForMember(ae => ae.FileType, opt =>
            {
                opt.PreCondition(aae => aae.File != null);
                opt.MapFrom(aae => FileTypeMapper.MapFileTypeFromExt(Path.GetExtension(aae.File!.FileName)));
            });

        CreateMap<(string UserId, IFormFile? File), AddAvatarEntry>()
            .ForMember(aae => aae.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(aae => aae.File, opt => opt.MapFrom(src => src.File));

        CreateMap<(string UserId, string FriendId), FriendEntry>()
            .ForMember(fe => fe.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(fe => fe.FriendId, opt => opt.MapFrom(src => src.FriendId));
    }
}