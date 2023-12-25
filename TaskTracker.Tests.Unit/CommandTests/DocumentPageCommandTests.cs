using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker.Application.DocumentPage.Command;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Tests.Unit.CommandTests
{
    public class DocumentPageCommandTests
    {
        [Fact]
        public async Task UpdateDocumentPageCommand_LastModifiedTimestampUpdated()
        {
            var command = new UpdateDocumentPageCommand
            {
                Id = 1,
            };

            var page = new TaskTrackerDocumentPage();

            var repository = Substitute.For<IDocumentPageRepository>();

            repository.GetByIdAsync(command.Id, Arg.Any<Func<TaskTrackerDocumentPage, TaskTrackerDocumentPage>>())
                .Returns(page);

            repository.UpdateAsync(page).Returns(Task.CompletedTask);

            var validator = Substitute.For<IValidator<UpdateDocumentPageCommand>>();

            validator.ValidateAsync(command).Returns(new ValidationResult());

            var handler = new UpdateDocumentPageHandler(repository, validator);

            var res = await handler.Handle(command, default);

            await repository.Received(1).UpdateAsync(page);

            Assert.True(res.IsSuccess);
            Assert.NotEqual(default, page.LastModifiedTimestamp);
        }
    }
}
