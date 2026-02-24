namespace NZWalks.Controllers.Models.DTO
{
    public class RegionUpdateRequestDto
    {
        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
