using AutoMapper;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.AutoMapper;

/// <summary>
/// User type auto mapper
/// </summary>
public class UnitTypeAutoMapperProfile : Profile
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public UnitTypeAutoMapperProfile()
    {
        CreateMap<UnitTypeEntry, GetUnitTypeEntry>();
        CreateMap<UnitTypeEntry, UnitTypeShortEntry>();
        CreateMap<UpsertUnitTypeEntry, UnitTypeEntry>()
            .ForMember(ute => ute.IsActive, opt => opt.MapFrom(_ => true));
    }
}