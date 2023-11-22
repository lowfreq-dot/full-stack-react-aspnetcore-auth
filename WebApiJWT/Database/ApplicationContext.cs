using Microsoft.EntityFrameworkCore;
using WebApiJWT.Models;

namespace WebApiJWT.Database
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated(); // гарантируем, что бд будет создана
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { optionsBuilder.UseSqlite(@"Data Source=App.db"); }
    }
}
