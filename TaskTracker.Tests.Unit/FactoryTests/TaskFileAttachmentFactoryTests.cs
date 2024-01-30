using TaskTracker.Application;

namespace TaskTracker.Tests.Unit.FactoryTests
{
    public class TaskFileAttachmentFactoryTests
    {
        private readonly TaskFileAttachmentFactory _factory;

        public TaskFileAttachmentFactoryTests()
        {
            _factory = new TaskFileAttachmentFactory();
        }

        [Fact]
        public void Create_CreatesValid_Entity()
        {
            const string FileName = "file";
            const long TaskId = 1;

            var file = _factory.Create(TaskId, FileName);

            Assert.Equal(FileName, file.FileName);
            Assert.Equal(TaskId, file.TaskId);
        }
    }
}
