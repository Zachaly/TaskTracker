using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Id);

            modelBuilder.Entity<User>()
                .Property(u => u.Email).HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(u => u.FirstName).HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.LastName).HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash);

            modelBuilder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);
        }
    }
}
