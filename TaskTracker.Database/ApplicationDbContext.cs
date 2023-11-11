using Microsoft.EntityFrameworkCore;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserTask> Tasks { get; set; }
        public DbSet<TaskList> TaskLists { get; set; }

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

            modelBuilder.Entity<UserTask>()
                .HasOne(t => t.Creator)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CreatorId);

            modelBuilder.Entity<UserTask>()
                .Property(t => t.Description).HasMaxLength(1000);

            modelBuilder.Entity<UserTask>()
                .Property(t => t.Title).HasMaxLength(200);

            modelBuilder.Entity<TaskList>()
                .HasOne(l => l.Creator)
                .WithMany(u => u.Lists)
                .HasForeignKey(l => l.CreatorId);

            modelBuilder.Entity<TaskList>()
                .HasMany(l => l.Tasks)
                .WithOne(t => t.List)
                .HasForeignKey(t => t.ListId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<TaskList>()
                .Property(l => l.Description).HasMaxLength(1000);

            modelBuilder.Entity<TaskList>()
                .Property(l => l.Title).HasMaxLength(200);
        }
    }
}
