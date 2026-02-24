using NZWalks.Controllers.Models.Domain;

namespace NZWalks.Controllers.Models.DTO
{
    public class WalkDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Length { get; set; }
        public string? walkImageUrl { get; set; }

        public RegionDto Region { get; set; }
        public DifficultyDto Difficulty { get; set; }
    }
}
