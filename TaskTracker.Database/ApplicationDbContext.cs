using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Id).HasColumnName("id");

            modelBuilder.Entity<User>()
                .Property(u => u.Email).HasColumnName("email").HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(u => u.FirstName).HasColumnName("first_name").HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.LastName).HasColumnName("last_name").HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash).HasColumnName("password_hash");
        }
    }
}
