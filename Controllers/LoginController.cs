using Microsoft.AspNetCore.Mvc;
using POS_API.DTOs;
using POS_API.Services;

namespace POS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;
        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var token = _loginService.Authenticate(loginRequestDTO);
                return Ok(new { AccessToken = token });
            }
            catch (UnauthorizedAccessException ex)
            {

                return Unauthorized(new { message = ex.Message });
            }
        }

    }
}
