using Microsoft.AspNetCore.Mvc;
using NZWalks.Controllers.Models.DTO;

namespace NZWalks.Repositories
{
    public interface IAuthRepository
    {
        Task<IActionResult> Register(AuthDTO registerDto);
    }
}