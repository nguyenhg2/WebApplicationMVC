using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplicationMVC.Common;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null
                || string.IsNullOrEmpty(request.Username)
                || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { message = "Tài khoản và mật khẩu không được để trống" });
            }

            var secretKey = _config["Jwt:SecretKey"] ?? "YourSecretKeyForAuthenticationShouldBeLongEnough";
            var expireMinutes = int.Parse(_config["Jwt:ExpireMinutes"] ?? "60");

            var token = TokenHelper.GenerateToken(
                secretKey,
                expireMinutes,
                "1",
                request.Username,
                "User");

            var response = new UserResponse
            {
                Token = token,
                UserId = "1",
                Username = request.Username,
                Name = "Example",
                Email = "example@mail.com",
                Role = "User",
                ExpiresIn = (expireMinutes * 60).ToString()
            };

            return Ok(response);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
