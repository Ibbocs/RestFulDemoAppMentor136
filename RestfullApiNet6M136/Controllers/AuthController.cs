using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestfullApiNet6M136.Abstraction.Services;

namespace RestfullApiNet6M136.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthoService authoService;

        public AuthController(IAuthoService _authoService)
        {
            this.authoService = _authoService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(string userNameOrEmail= "Ibbbo", string password= "Ibbb!234")
        {

            var data = await authoService.LoginAsync(userNameOrEmail, password);
            return StatusCode(data.StatusCode, data);
        }

        [HttpPost("refresh-token-login")]
        public async Task<IActionResult> RefreshTokenLogin(string refreshToken)
        {
            var data = await authoService.LoginWithRefreshTokenAsync(refreshToken);
            return StatusCode(data.StatusCode, data);
        }

        [HttpPut("[action]")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> LogOut(string userNameOrEmail)
        {
            var data = await authoService.LogOut(userNameOrEmail);
            return StatusCode(data.StatusCode, data);
        }

        [HttpPost("password-reset-token")]
        public async Task<IActionResult> PasswordReset(string email, string currentPas, string newPas)
        {
            var data = await authoService.PasswordResetAsnyc(email, currentPas,newPas);
            return StatusCode(data.StatusCode, data);
        }

    }
}
