namespace NZWalks.Controllers.Models.Domain
{
    public class UserSavedWalk
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid WalkId { get; set; }

        public Walk Walk { get; set; }
    }
}
