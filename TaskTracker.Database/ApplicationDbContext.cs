using Microsoft.EntityFrameworkCore;

namespace TaskTracker.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
