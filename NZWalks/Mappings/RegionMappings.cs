using AutoMapper;
using NZWalks.Controllers.Models.Domain;
using NZWalks.Controllers.Models.DTO;

namespace NZWalks.Mappings
{
    public class RegionMappings:Profile
    {
        public RegionMappings()
        {
            CreateMap<RegionDto, Region>();
            CreateMap<Region, RegionDto>();
            CreateMap<RegionUpdateRequestDto, Region>();
            CreateMap<Region, RegionUpdateRequestDto>();
        }
    }
}
