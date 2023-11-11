using Bogus;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Tests.Integration
{
    public static class FakeDataFactory
    {
        public static List<User> GenerateUsers(int count)
            => new Faker<User>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.PasswordHash, f => f.Random.AlphaNumeric(20))
                .Generate(count);

        public static List<UserTask> GenerateUserTasks(int count, long userId, long? listId = null)
            => new Faker<UserTask>()
                .RuleFor(t => t.Title, f => f.Random.AlphaNumeric(20))
                .RuleFor(t => t.Description, f => f.Random.AlphaNumeric(40))
                .RuleFor(t => t.CreatorId, _ =>  userId)
                .RuleFor(t => t.CreationTimestamp, f => f.Random.Number(100))
                .RuleFor(t => t.ListId, _ => listId)
                .Generate(count);

        public static List<TaskList> GenerateTaskLists(int count, long userId)
            => new Faker<TaskList>()
                .RuleFor(l => l.Color, f => f.Random.AlphaNumeric(5))
                .RuleFor(l => l.Title, f => f.Random.AlphaNumeric(50))
                .RuleFor(l => l.Description, f => f.Random.AlphaNumeric(100))
                .RuleFor(l => l.CreatorId, _ => userId)
                .Generate(count);
    }
}
