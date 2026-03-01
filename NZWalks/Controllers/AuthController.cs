using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Controllers.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthRepository authRepository;
        
        public AuthController(IAuthRepository authRepository)
        {
            this.authRepository = authRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] AuthDTO registerDto)
        {
            AuthDTO authDTO = new AuthDTO()
            {
                UserName = registerDto.UserName,
                Password = registerDto.Password,
                Roles = registerDto.Roles
            };
            return await authRepository.Register(authDTO);
            
        }
    }
}
