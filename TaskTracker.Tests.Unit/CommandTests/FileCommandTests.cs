using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TaskTracker.Api.Infrastructure;
using TaskTracker.Api.Infrastructure.Command;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;

namespace TaskTracker.Tests.Unit.CommandTests
{
    public class FileCommandTests
    {
        [Fact]
        public async Task SaveProfilePictureCommand_UserWithNoPicture_FileSpecified_PictureUpdated()
        {
            var file = Substitute.For<IFormFile>();
            file.FileName.Returns("file.jpg");

            var command = new SaveProfilePictureCommand
            {
                UserId = 1,
                File = file
            };

            var user = new User
            {
                ProfilePicture = null
            };

            const string FileName = "filename";
            var fileService = Substitute.For<IFileService>();
            fileService.SaveProfilePictureAsync(file).Returns(FileName);

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByIdAsync(command.UserId, Arg.Any<Func<User, User>>()).Returns(user);
            userRepository.UpdateAsync(user).Returns(Task.CompletedTask);

            var configData = new Dictionary<string, string>
            {
                { "Image:ValidExtensions:0", "jpg"}
            };

            var config = new ConfigurationBuilder().AddInMemoryCollection(configData!).Build();

            var handler = new SaveProfilePictureHandler(userRepository, fileService, config);

            var res = await handler.Handle(command, default);

            await userRepository.Received(1).UpdateAsync(user);

            Assert.True(res.IsSuccess);
            Assert.Equal(FileName, user.ProfilePicture);
        }

        [Fact]
        public async Task SaveProfilePictureCommand_UserWithPicture_FileSpecified_PictureUpdated()
        {
            var file = Substitute.For<IFormFile>();
            file.FileName.Returns("file.jpg");

            var command = new SaveProfilePictureCommand
            {
                UserId = 1,
                File = file
            };

            const string OldPicture = "oldPic";

            var user = new User
            {
                ProfilePicture = OldPicture
            };

            const string FileName = "filename";
            var fileService = Substitute.For<IFileService>();
            fileService.SaveProfilePictureAsync(file).Returns(FileName);
            fileService.DeleteProfilePictureAsync(OldPicture).Returns(Task.CompletedTask);

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByIdAsync(command.UserId, Arg.Any<Func<User, User>>()).Returns(user);
            userRepository.UpdateAsync(user).Returns(Task.CompletedTask);

            var configData = new Dictionary<string, string>
            {
                { "Image:ValidExtensions:0", "jpg" }
            };

            var config = new ConfigurationBuilder().AddInMemoryCollection(configData!).Build();

            var handler = new SaveProfilePictureHandler(userRepository, fileService, config);

            var res = await handler.Handle(command, default);

            await userRepository.Received(1).UpdateAsync(user);
            await fileService.Received(1).DeleteProfilePictureAsync(OldPicture);

            Assert.True(res.IsSuccess);
            Assert.Equal(FileName, user.ProfilePicture);
        }

        [Fact]
        public async Task SaveProfilePictureCommand_UserWithPicture_FileNotSpecified_PictureSetToNull()
        {
            var command = new SaveProfilePictureCommand
            {
                UserId = 1,
                File = null
            };

            const string OldPicture = "oldPic";

            var user = new User
            {
                ProfilePicture = OldPicture
            };

            var fileService = Substitute.For<IFileService>();
            fileService.DeleteProfilePictureAsync(OldPicture).Returns(Task.CompletedTask);

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByIdAsync(command.UserId, Arg.Any<Func<User, User>>()).Returns(user);
            userRepository.UpdateAsync(user).Returns(Task.CompletedTask);

            var configData = new Dictionary<string, string>
            {
                { "Image:ValidExtensions:0", "jpg" }
            };

            var config = new ConfigurationBuilder().AddInMemoryCollection(configData!).Build();

            var handler = new SaveProfilePictureHandler(userRepository, fileService, config);

            var res = await handler.Handle(command, default);

            await userRepository.Received(1).UpdateAsync(user);
            await fileService.Received(1).DeleteProfilePictureAsync(OldPicture);

            Assert.True(res.IsSuccess);
            Assert.Null(user.ProfilePicture);
        }

        [Fact]
        public async Task SaveProfilePictureCommand_InvalidFileExtension_Failure()
        {
            var file = Substitute.For<IFormFile>();
            file.FileName.Returns("file.pdf");

            var command = new SaveProfilePictureCommand
            {
                UserId = 1,
                File = file
            };

            var fileService = Substitute.For<IFileService>();

            var userRepository = Substitute.For<IUserRepository>();

            var configData = new Dictionary<string, string>
            {
                { "Image:ValidExtensions:0", "jpg"}
            };

            var config = new ConfigurationBuilder().AddInMemoryCollection(configData!).Build();

            var handler = new SaveProfilePictureHandler(userRepository, fileService, config);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
        }

        [Fact]
        public async Task SaveProfilePictureCommand_UserNotFound_Failure()
        {
            var file = Substitute.For<IFormFile>();
            file.FileName.Returns("file.jpg");

            var command = new SaveProfilePictureCommand
            {
                UserId = 1,
                File = file
            };

            var fileService = Substitute.For<IFileService>();

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByIdAsync(command.UserId, Arg.Any<Func<User, User>>()).ReturnsNull();

            var configData = new Dictionary<string, string>
            {
                { "Image:ValidExtensions:0", "jpg"}
            };

            var config = new ConfigurationBuilder().AddInMemoryCollection(configData!).Build();

            var handler = new SaveProfilePictureHandler(userRepository, fileService, config);

            var res = await handler.Handle(command, default);

            Assert.False(res.IsSuccess);
        }
    }
}
