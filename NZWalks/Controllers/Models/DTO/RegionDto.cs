using System.ComponentModel.DataAnnotations;

namespace NZWalks.Controllers.Models.DTO
{
    public class RegionDto
    {
        public Guid Id { get; set; }
        [Required]
        [MinLength(3,ErrorMessage ="Minimum length of code is 3")]
        [MaxLength(6, ErrorMessage = "Maximum length of code is 6")]
        public string Code { get; set; }
        [Required]
        [MaxLength(10, ErrorMessage = "Maximum length of Name field is 10")]
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
