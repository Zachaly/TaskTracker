using System.Net;
using System.Net.Http.Json;
using TaskTracker.Model.TaskFileAttachment;

namespace TaskTracker.Tests.Integration.ApiTests
{
    public class TaskFileAttachmentControllerTests : ApiTest
    {
        const string Endpoint = "/api/task-file-attachment";

        [Fact]
        public async Task GetAsync_ReturnsAttachments()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var groupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, groupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var list = FakeDataFactory.GenerateTaskLists(1, userId, groupId, space.Id).First();

            _dbContext.TaskLists.Add(list);
            _dbContext.SaveChanges();

            var task = FakeDataFactory.GenerateUserTasks(1, userId, list.Id, _dbContext.UserTaskStatuses.First().Id).First();

            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();

            var attachments = FakeDataFactory.GenerateTaskFileAttachments(5, task.Id);

            _dbContext.TaskFileAttachments.AddRange(attachments);
            _dbContext.SaveChanges();

            var response = await _httpClient.GetAsync(Endpoint);

            var content = await response.Content.ReadFromJsonAsync<IEnumerable<TaskFileAttachmentModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equivalent(attachments.Select(x => x.Id), content.Select(x => x.Id));
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsCorrectAttachment()
        {
            var userId = (await AuthorizeAsync()).UserData!.Id;

            var groupId = _dbContext.TaskStatusGroups.First().Id;

            var space = FakeDataFactory.GenerateUserSpaces(1, userId, groupId).First();

            _dbContext.UserSpaces.Add(space);
            _dbContext.SaveChanges();

            var list = FakeDataFactory.GenerateTaskLists(1, userId, groupId, space.Id).First();

            _dbContext.TaskLists.Add(list);
            _dbContext.SaveChanges();

            var task = FakeDataFactory.GenerateUserTasks(1, userId, list.Id, _dbContext.UserTaskStatuses.First().Id).First();

            _dbContext.Tasks.Add(task);
            _dbContext.SaveChanges();

            var attachments = FakeDataFactory.GenerateTaskFileAttachments(5, task.Id);

            _dbContext.TaskFileAttachments.AddRange(attachments);
            _dbContext.SaveChanges();

            var attachment = attachments.First();

            var response = await _httpClient.GetAsync($"{Endpoint}/{attachment.Id}");

            var content = await response.Content.ReadFromJsonAsync<TaskFileAttachmentModel>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(attachment.FileName, content.FileName);
            Assert.Equal(attachment.Id, content.Id);
            Assert.Equal(attachment.FileName[attachment.FileName.LastIndexOf('.')..], content.Type);
        }

        [Fact]
        public async Task GetByIdAsync_NoAttachmentWithSpecifiedId_ReturnsNotFound()
        {
            await AuthorizeAsync();

            var response = await _httpClient.GetAsync($"{Endpoint}/2137");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
