using Microsoft.AspNetCore.Identity;
using NZWalks.Controllers.Models.Domain;
using NZWalks.Controllers.Models.DTO;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
namespace NZWalks.Repositories
{
    public class SqlAuthRepository : IAuthRepository
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<IdentityUser> userManager;

        public SqlAuthRepository(Microsoft.AspNetCore.Identity.UserManager<IdentityUser> usermanager)
        {
            this.userManager = userManager;
        }

        public async Task<IActionResult> Register(AuthDTO registerDto)
        {
            var identyUser = new IdentityUser()
            {
                UserName = registerDto.UserName,
                Email = registerDto.UserName
            };

            var identityResult = await userManager.CreateAsync(identyUser, registerDto.Password);

            if (identityResult.Succeeded)
            {
                identityResult = await userManager.AddToRolesAsync(identyUser, registerDto.Roles);

                if (identityResult.Succeeded)
                {
                    return new OkObjectResult("User was registered! Please login");
                }
            }

            return new BadRequestObjectResult("Something went worng");
        }

    }
}
