using AutoMapper;
using NZWalks.Controllers.Models.Domain;
using NZWalks.Controllers.Models.DTO;

namespace NZWalks.Mappings
{
    public class WalkMappings : Profile
    {
        public WalkMappings()
        {
            CreateMap<WalkDto, Walk>();
            CreateMap<Walk, WalkDto>();
            CreateMap<Difficulty, DifficultyDto>();
            CreateMap<DifficultyDto, Difficulty>();
        }
    }
}
