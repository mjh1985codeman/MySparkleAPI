using Microsoft.EntityFrameworkCore;
using my_sparkle_api.Models;

namespace my_sparkle_api.Data
{
    // The AppDbContext is the "bridge" between your C# classes and your SQL Server database.
    // EF Core uses this class to query and save data.
    public class AppDbContext : DbContext
    {
        // This constructor receives options (like connection strings) from the system at startup.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets represent database tables.
        // EF Core will create tables named 'Users' and 'Children' based on these models.
        //
        // EF Core (Entity Framework Core) is an ORM — an Object Relational Mapper.
        // It helps keep the code (C# models) and database schema in sync.
        // Whenever I make changes to my models, I can run a "migration" to update the database structure.
        // Once the database is set up, EF Core automatically handles all SQL operations
        // (like inserting, updating, deleting, and reading records) behind the scenes.

        public DbSet<User> Users { get; set; }
        public DbSet<Child> Children { get; set; }

        public DbSet<Allergy> Allergies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // One-to-many relationship: one User has many Children
            modelBuilder.Entity<User>()
                .HasMany(u => u.Children)
                .WithOne(c => c.Parent)
                .HasForeignKey(c => c.UserId);
        }
    }
}
