using Microsoft.EntityFrameworkCore;
using URLShortenerApiApplication.Entities;

namespace URLShortenerApiApplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<URL> Urls { get; set; }

    }
    
}

