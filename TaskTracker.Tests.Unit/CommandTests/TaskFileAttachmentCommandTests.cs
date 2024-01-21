using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Api.Infrastructure.Command;
using TaskTracker.Application;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.TaskFileAttachment;

namespace TaskTracker.Tests.Unit.CommandTests
{
    public class TaskFileAttachmentCommandTests
    {
        [Fact]
        public async Task AddTaskFileAttachmentCommand_Success()
        {
            var mockedFiles = new List<IFormFile>
            {
                Substitute.For<IFormFile>(),
                Substitute.For<IFormFile>()
            };

            var command = new AddTaskFileAttachmentsCommand
            {
                TaskId = 1,
                Files = mockedFiles
            };

            var files = new List<TaskFileAttachment>();

            var repository = Substitute.For<ITaskFileAttachmentRepository>();

            repository.AddAsync(Arg.Any<TaskFileAttachment[]>()).Returns(Task.CompletedTask)
                .AndDoes(info => files.AddRange(info.Arg<TaskFileAttachment[]>()));

            var factory = Substitute.For<ITaskFileAttachmentFactory>();

            factory.Create(command.TaskId, Arg.Any<string>())
                .Returns(info => new TaskFileAttachment
                {
                    TaskId = command.TaskId,
                    FileName = info.Arg<string>(),
                });

            var fileNames = new string[] { "file1", "file2" };

            var fileService = Substitute.For<IFileService>();

            fileService.SaveTaskFileAttachmentsAsync(Arg.Any<IFormFile[]>())
                .Returns(fileNames);

            var handler = new AddTaskFileAttachmentsHandler(repository, factory, fileService);

            var res = await handler.Handle(command, default);

            Assert.True(res.IsSuccess);
            Assert.Equivalent(fileNames, files.Select(x => x.FileName));
        }

        [Fact]
        public async Task AddTaskFileAttachmentCommand_ExceptionThrown_Failure()
        {
            var mockedFiles = new List<IFormFile>
            {
                Substitute.For<IFormFile>(),
                Substitute.For<IFormFile>()
            };

            var command = new AddTaskFileAttachmentsCommand
            {
                TaskId = 1,
                Files = mockedFiles
            };

            var repository = Substitute.For<ITaskFileAttachmentRepository>();

            var factory = Substitute.For<ITaskFileAttachmentFactory>();

            var fileService = Substitute.For<IFileService>();

            const string Error = "error";

            fileService.SaveTaskFileAttachmentsAsync(Arg.Any<IFormFile[]>())
                .Throws(new Exception(Error));

            var handler = new AddTaskFileAttachmentsHandler(repository, factory, fileService);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
            Assert.Equal(Error, res.Error);
        }

        [Fact]
        public async Task DeleteTaskFileAttachmentByIdCommand_Success()
        {
            var command = new DeleteTaskFileAttachmentByIdCommand(1);

            var file = new TaskFileAttachment
            {
                FileName = "file"
            };

            var repository = Substitute.For<ITaskFileAttachmentRepository>();

            repository.GetByIdAsync(command.Id)
                .Returns(_ => new TaskFileAttachmentModel { FileName = file.FileName });

            repository.DeleteByIdAsync(command.Id).Returns(Task.CompletedTask);

            var fileService = Substitute.For<IFileService>();

            fileService.DeleteTaskFileAttachmentAsync(file.FileName).Returns(Task.CompletedTask);

            var handler = new DeleteTaskFileAttachmentByIdHandler(repository, fileService);

            var res = await handler.Handle(command, default);

            await fileService.Received(1).DeleteTaskFileAttachmentAsync(file.FileName);
            await repository.Received(1).DeleteByIdAsync(command.Id);

            Assert.True(res.IsSuccess);
        }

        [Fact]
        public async Task DeleteTaskFileAttachmentByIdCommand_FileNotFound_Failure()
        {
            var command = new DeleteTaskFileAttachmentByIdCommand(1);

            var repository = Substitute.For<ITaskFileAttachmentRepository>();

            repository.GetByIdAsync(command.Id)
                .ReturnsNull();

            var fileService = Substitute.For<IFileService>();

            var handler = new DeleteTaskFileAttachmentByIdHandler(repository, fileService);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
        }
    }
}
