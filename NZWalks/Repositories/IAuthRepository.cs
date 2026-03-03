using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Controllers.Models.DTO;

namespace NZWalks.Repositories
{
    public interface IAuthRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}