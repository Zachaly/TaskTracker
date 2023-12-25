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
        public DbSet<TaskStatusGroup> TaskStatusGroups { get; set; }
        public DbSet<UserTaskStatus> UserTaskStatuses { get; set; }
        public DbSet<UserSpace> UserSpaces { get; set; }
        public DbSet<TaskTrackerDocument> Documents { get; set; }
        public DbSet<TaskTrackerDocumentPage> DocumentPages { get; set; }

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

            modelBuilder.Entity<UserTaskStatus>()
                .Property(s => s.Color).HasMaxLength(10);

            modelBuilder.Entity<UserTaskStatus>()
                .Property(s => s.Name).HasMaxLength(100);

            modelBuilder.Entity<TaskStatusGroup>()
                .Property(g => g.Name).HasMaxLength(100);

            modelBuilder.Entity<UserTaskStatus>()
                .HasOne(s => s.Group)
                .WithMany(g => g.Statuses)
                .HasForeignKey(s => s.GroupId);

            modelBuilder.Entity<UserTask>()
                .HasOne(t => t.Status)
                .WithMany(s => s.Tasks)
                .HasForeignKey(t => t.StatusId);

            modelBuilder.Entity<TaskStatusGroup>()
                .HasOne(g => g.User)
                .WithMany(u => u.StatusGroups)
                .HasForeignKey(g => g.UserId);

            modelBuilder.Entity<TaskList>()
                .HasOne(l => l.TaskStatusGroup)
                .WithMany(g => g.Lists)
                .HasForeignKey(l => l.TaskStatusGroupId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Spaces)
                .WithOne(s => s.Owner)
                .HasForeignKey(s => s.OwnerId);

            modelBuilder.Entity<UserSpace>()
                .HasMany(s => s.Lists)
                .WithOne(l => l.Space)
                .HasForeignKey(l => l.SpaceId)
                .OnDelete(DeleteBehavior.ClientCascade);

            modelBuilder.Entity<UserSpace>()
                .Property(s => s.Title).HasMaxLength(100);

            modelBuilder.Entity<TaskStatusGroup>()
                .HasMany(g => g.Spaces)
                .WithOne(s => s.StatusGroup)
                .HasForeignKey(s => s.StatusGroupId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Documents)
                .WithOne(d => d.Creator)
                .HasForeignKey(d => d.CreatorId);

            modelBuilder.Entity<UserSpace>()
                .HasMany(s => s.Documents)
                .WithOne(d => d.Space)
                .HasForeignKey(d => d.SpaceId);

            modelBuilder.Entity<TaskTrackerDocument>()
                .HasMany(d => d.Pages)
                .WithOne(p => p.Document)
                .HasForeignKey(p => p.DocumentId);
        }
    }
}
