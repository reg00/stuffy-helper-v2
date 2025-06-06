using AutoMapper;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Minio.Features.Common;
using StuffyHelper.Minio.Features.Helpers;

namespace StuffyHelper.Contracts.AutoMapper;

public class MediaAutoMapperProfile : Profile
{
    public MediaAutoMapperProfile()
    {
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
    }
}