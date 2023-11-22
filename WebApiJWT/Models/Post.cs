using System.ComponentModel.DataAnnotations;

namespace WebApiJWT.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public User? User { get; set; }
        public string? Title { get; set; }
        public string? Body { get; set; }
        public DateTime CreatedOn { get; set; }

        public Post() { }

        public Post(User User, CreatePostDto dto)
        {
            this.Title = dto.Title;
            this.Body = dto.Body;
            this.User = User;
            this.CreatedOn = DateTime.UtcNow;
        }
    }

    public class CreatePostDto
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
    }
}
