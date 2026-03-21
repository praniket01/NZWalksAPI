using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Controllers.Models.Domain;
using NZWalks.Controllers.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IAuthRepository authRepository;

        
        public AuthController(UserManager<ApplicationUser> userManager, IAuthRepository authRepository)
        {
            this.userManager = userManager;
            this.authRepository = authRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] AuthDTO registerDto)
        {
            var identityUSer = new ApplicationUser()
            {
                UserName = registerDto.UserName,
                Email = registerDto.UserName,
                Name = registerDto.Name
            };
           
            var identityResult = await userManager.CreateAsync(identityUSer, registerDto.Password);

            if (identityResult.Succeeded)
            {
                if(registerDto.Roles != null || registerDto.Roles.Length != 0)
                {
                    identityResult = await userManager.AddToRolesAsync(identityUSer, registerDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return new OkObjectResult("User was registered! Please login");
                    }
                }
                
            }
            return new BadRequestObjectResult(identityResult.Errors.ToList().FirstOrDefault().Description);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginRequest)
        {
            if(loginRequest.username == null || loginRequest.password == null)
            {
                return BadRequest("Please enter credentials");
            }
            var checkUserPresent = await userManager.FindByEmailAsync(loginRequest.username);
            
            if(checkUserPresent != null)
            {
                var isPasswordCorrect = await userManager.CheckPasswordAsync(checkUserPresent, loginRequest.password);
                
                var roles = await userManager.GetRolesAsync(checkUserPresent);
    
                if (isPasswordCorrect && roles != null)
                {
                    string jwt = authRepository.CreateJwtToken(checkUserPresent, roles.ToList());

                    LoginResponseDto loginResponseDto = new LoginResponseDto()
                    {
                        jwt = jwt
                    };

                    return Ok(loginResponseDto);
                }
            }

            return BadRequest("Please enter correct credentials");
        }
    }
}
