using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebApiJWT.Database;
using WebApiJWT.Models;

namespace WebApiJWT.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : Controller
    {
        [HttpGet]
        public IActionResult GetAllPosts()
        {
            using var db = new ApplicationContext();

            // Получаем все посты, предварительно загружая данные о пользователе
            var posts = db.Posts.Include(p => p.User).OrderByDescending(s => s.CreatedOn).ToList();

            // Преобразуем данные о постах, оставляя только идентификатор пользователя
            var postsDto = posts.Select(p => new
            {
                PostId = p.Id,
                Title = p.Title,
                Body = p.Body,
                UserId = p.User?.Id, // Используем ?. для обеспечения безопасного доступа к свойству User
                CreatedOn = p.CreatedOn,
            }).ToList();

            return Ok(postsDto);
        }

        [HttpPost]
        [Authorize]
        public IActionResult CreatePost([FromBody] CreatePostDto dto)
        {
            using var db = new ApplicationContext();

            var userEmail = HttpContext.User.FindFirst(ClaimTypes.Name)?.Value;

            User? user = db.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
            {
                return NotFound($"User with email {userEmail} not found");
            }

            Post post = new(user, dto);

            db.Posts.Add(post);
            db.SaveChanges();

            return Created("/posts{id}", dto);
        }
    }
}
