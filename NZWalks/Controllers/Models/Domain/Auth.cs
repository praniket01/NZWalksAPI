using System.ComponentModel.DataAnnotations;

namespace NZWalks.Controllers.Models.Domain
{
    public class Auth
    {
        [Required]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
