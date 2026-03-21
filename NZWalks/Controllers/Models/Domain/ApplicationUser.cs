using Microsoft.AspNetCore.Identity;

namespace NZWalks.Controllers.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
