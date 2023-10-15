using Microsoft.EntityFrameworkCore;
using TaskTracker.Database;

namespace TaskTracker.Tests.Integration
{
    public class DatabaseTest : IDisposable
    {
        protected readonly ApplicationDbContext _dbContext;

        public DatabaseTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(Constants.ConnectionString).Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.Migrate();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
