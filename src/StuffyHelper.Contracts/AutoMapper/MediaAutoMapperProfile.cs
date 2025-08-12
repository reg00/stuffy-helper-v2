using AutoMapper;
using Microsoft.AspNetCore.Http;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Minio.Features.Common;
using StuffyHelper.Minio.Features.Helpers;

namespace StuffyHelper.Contracts.AutoMapper;

/// <summary>
/// Media auto mapper profile
/// </summary>
public class MediaAutoMapperProfile : Profile
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public MediaAutoMapperProfile()
    {
        CreateMap<(Guid EventId, IFormFile File), AddMediaEntry>()
            .ForMember(ame => ame.EventId, opt => opt.MapFrom(src => src.EventId))
            .ForMember(ame => ame.File, opt => opt.MapFrom(src => src.File))
            .ForMember(ame => ame.MediaType, opt => opt.MapFrom(_ => MediaType.Image))
            .ForMember(ame => ame.Link, opt => opt.MapFrom(_ => string.Empty));
        CreateMap<MediaEntry, GetMediaEntry>();
        CreateMap<MediaEntry, MediaShortEntry>();
        CreateMap<(Stream? Stream, MediaEntry Avatar), MediaBlobEntry>()
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
        CreateMap<(AddMediaEntry Media, bool IsPrimal), MediaEntry>()
            .ForMember(me => me.EventId, opt => opt.MapFrom(src => src.Media.EventId))
            .ForMember(me => me.FileName, opt => opt.MapFrom(src => src.Media.File != null ? Path.GetFileNameWithoutExtension(src.Media.File.FileName) : string.Empty))
            .ForMember(me => me.FileType, opt => opt.MapFrom(src => src.Media.File != null ? FileTypeMapper.MapFileTypeFromExt(Path.GetExtension(src.Media.File.FileName)) : FileType.Link))
            .ForMember(me => me.MediaType, opt => opt.MapFrom(src => src.Media.MediaType))
            .ForMember(me => me.Link, opt => opt.MapFrom(src => src.Media.Link))
            .ForMember(me => me.IsPrimal, opt => opt.MapFrom(src => src.IsPrimal));
    }
}