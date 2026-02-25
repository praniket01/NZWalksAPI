using System.ComponentModel.DataAnnotations;
using NZWalks.Controllers.Models.Domain;

namespace NZWalks.Controllers.Models.DTO
{
    public class WalkDto
    {
        public Guid? Id { get; set; }
        [MinLength(3, ErrorMessage = "Minimum length of Name is 3")]
        [MaxLength(6, ErrorMessage = "Maximum length of Name is 6")]
        public string Name { get; set; }
        [MaxLength(100, ErrorMessage ="Maximum length of description field is 100 words")]
        public string Description { get; set; }
        public double Length { get; set; }
        public string? walkImageUrl { get; set; }

        public RegionDto Region { get; set; }
        public DifficultyDto Difficulty { get; set; }
    }
}
