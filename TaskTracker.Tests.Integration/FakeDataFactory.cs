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

        public static List<UserTask> GenerateUserTasks(int count, long userId, long listId, long statusId)
            => new Faker<UserTask>()
                .RuleFor(t => t.Title, f => f.Random.AlphaNumeric(20))
                .RuleFor(t => t.Description, f => f.Random.AlphaNumeric(40))
                .RuleFor(t => t.CreatorId, _ =>  userId)
                .RuleFor(t => t.CreationTimestamp, f => f.Random.Number(100))
                .RuleFor(t => t.ListId, _ => listId)
                .RuleFor(t => t.StatusId, _ => statusId)
                .Generate(count);

        public static List<TaskList> GenerateTaskLists(int count, long userId, long groupId, long spaceId)
            => new Faker<TaskList>()
                .RuleFor(l => l.Color, f => f.Random.AlphaNumeric(5))
                .RuleFor(l => l.Title, f => f.Random.AlphaNumeric(50))
                .RuleFor(l => l.Description, f => f.Random.AlphaNumeric(100))
                .RuleFor(l => l.CreatorId, _ => userId)
                .RuleFor(l => l.TaskStatusGroupId, _ => groupId)
                .RuleFor(l => l.SpaceId, _ => spaceId)
                .Generate(count);

        public static List<UserTaskStatus> GenerateTaskStatuses(int count, long groupId)
            => new Faker<UserTaskStatus>()
                .RuleFor(s => s.Color, f => f.Random.AlphaNumeric(5))
                .RuleFor(s => s.Name, f => f.Random.AlphaNumeric(50))
                .RuleFor(s => s.IsDefault, _ => false)
                .RuleFor(s => s.Index, f => f.Random.Number(0, 20))
                .RuleFor(s => s.GroupId, _ => groupId)
                .Generate(count);

        public static List<TaskStatusGroup> GenerateTaskStatusGroups(int count, long userId)
            => new Faker<TaskStatusGroup>()
                .RuleFor(g => g.Name, f => f.Random.AlphaNumeric(50))
                .RuleFor(g => g.UserId, _ => userId)
                .RuleFor(g => g.IsDefault, _ => false)
                .Generate(count);

        public static List<UserSpace> GenerateUserSpaces(int count, long userId, long statusGroupId)
            => new Faker<UserSpace>()
                .RuleFor(s => s.Title, f => f.Random.AlphaNumeric(20))
                .RuleFor(s => s.OwnerId, _ => userId)
                .RuleFor(s => s.StatusGroupId, _ => statusGroupId)
                .Generate(count);
    }
}
