using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApiJWT.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string? UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string? RefreshToken { get; set; }

        public User(string userName)
        {
            this.UserName = userName;
        }
        public User(UserRegistrationDto dto)
        {
            this.Email = dto.Email;
            this.Password = dto.Password;
        }

    }

    public class UserRegistrationDto
    {
        [EmailAddress]
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class UserLoginDto
    {
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}
