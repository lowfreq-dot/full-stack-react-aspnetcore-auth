using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApiJWT.Database;
using WebApiJWT.Models;

namespace WebApiJWT.Controllers
{
    [ApiController]
    [Route("/api")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("/api/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
        {
            using var db = new ApplicationContext();

            User? user = await db.Users
                .FirstOrDefaultAsync(p => p.Email == dto.Email && p.Password == dto.Password);

            if (user is null) return Unauthorized("Пользователь не найден");

            var accessToken = _authService.GenerateAccessToken(user);
            var refreshToken = _authService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            await db.SaveChangesAsync();

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.Add(AuthOptions.REFRESH_TOKEN_LIFETIME)
            });

            return Ok(new
            {
                accessToken,
                email = user.Email
            });
        }

        [HttpPost("/api/logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            using var db = new ApplicationContext();

            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            User? user = db.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return NotFound($"User with email {userEmail} not found");
            }

            // Generate new refresh token
            var newRefreshToken = _authService.GenerateRefreshToken();

            // Update user's refresh token in the database
            user.RefreshToken = newRefreshToken;
            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("/api/refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            // Get refresh token from the cookie
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest("Refresh token is missing.");
            }

            using var db = new ApplicationContext();

            // Find user by refresh token
            var user = await db.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user == null)
            {
                return Unauthorized("Invalid refresh token.");
            }

            // Generate new refresh token
            var newRefreshToken = _authService.GenerateRefreshToken();

            // Update user's refresh token in the database
            user.RefreshToken = newRefreshToken;
            await db.SaveChangesAsync();

            var accessToken = _authService.GenerateAccessToken(user);

            // Set the new refresh token in the cookie
            Response.Cookies.Append("refreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.Add(AuthOptions.REFRESH_TOKEN_LIFETIME)
            });

            return Ok(new
            {
                accessToken,
                email = user.Email
            });
        }


        [HttpPost("/api/registration")]
        public async Task<IActionResult> CreateUser([FromBody] UserRegistrationDto dto)
        {
            using ApplicationContext db = new ApplicationContext();

            if (await db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email) != null)
            {
                return Conflict("Пользователь с таким email уже существует!");
            }

            if (dto is null)
            {
                return BadRequest("Неправильные регистрационные данные!");
            }

            User user = new(dto);

            var result = db.Users.Add(user);
            await db.SaveChangesAsync();

            return Created("/", user);
        }



    }
}