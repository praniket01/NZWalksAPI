namespace NZWalks.Controllers.Models.Domain
{
    public class Walk
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Length { get; set; }
        public string? walkImageUrl { get; set; }

        public Guid RegionId { get; set; }
        public Region Region { get; set; }
        public Guid DifficultyId { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}
