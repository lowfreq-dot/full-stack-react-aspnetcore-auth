using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WebApiJWT.Database;
using WebApiJWT.Models;

namespace WebApiJWT.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly ILogger _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{userId}")]
        public IActionResult GetUser(int? userId)
        {

            if (userId is null || userId <= 0)
            {
                _logger.LogInformation("UserId: {userId}", userId);

                return BadRequest();
            }

            using var db = new ApplicationContext();

            var result = db.Users.Where(x => x.Id == userId).FirstOrDefault();

            if (result == null)
                return NotFound();

            return Ok(new { result.Email });
        }

        [HttpGet]
        //[Authorize(Roles = "admin")]
        [Authorize]
        public IActionResult GetAllUsers()
        {
            using var db = new ApplicationContext();

            var users = db.Users.ToList();

            return Ok(users);
        }
    }
}
