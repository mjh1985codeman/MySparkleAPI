using Microsoft.EntityFrameworkCore;
using MySparkleHeart.Api.Models;

namespace MySparkleHeart.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserProfile> Users { get; set; }
    }
}