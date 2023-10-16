using FluentValidation;
using FluentValidation.Results;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using TaskTracker.Application;
using TaskTracker.Application.Authorization.Service;
using TaskTracker.Application.Command;
using TaskTracker.Database.Repository;
using TaskTracker.Domain.Entity;
using TaskTracker.Model.User.Request;

namespace TaskTracker.Tests.Unit.Command
{
    public class UserCommandTests
    {
        [Fact]
        public async Task RegisterCommand_CreatesNewUser()
        {
            var users = new List<User>();

            var userFactory = Substitute.For<IUserFactory>();
            userFactory.Create(Arg.Any<RegisterRequest>(), Arg.Any<string>()).Returns(arg => new User
            {
                Email = arg.Arg<RegisterRequest>().Email,
                PasswordHash = arg.Arg<string>()
            });

            const string PasswordHash = "hash";

            var hashService = Substitute.For<IHashService>();
            hashService.HashStringAsync(Arg.Any<string>()).Returns(PasswordHash);

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(Arg.Any<string>()).ReturnsNull();

            const long NewId = 1;
            userRepository.AddAsync(Arg.Any<User>()).Returns(NewId).AndDoes(info =>
            {
                var user = info.Arg<User>();
                user.Id = NewId;
                users.Add(user);
            });

            var validator = Substitute.For<IValidator<RegisterCommand>>();

            validator.ValidateAsync(Arg.Any<RegisterCommand>()).Returns(Task.FromResult(new ValidationResult()));

            var handler = new RegisterCommandHandler(userRepository, hashService, userFactory, validator);

            var command = new RegisterCommand
            {
                Email = "email"
            };

            var response = await handler.Handle(command, default);

            Assert.True(response.IsSuccess);
            Assert.Equal(response.NewEntityId, NewId);
            Assert.Contains(users, x => x.Id == NewId && x.Email == command.Email && x.PasswordHash == PasswordHash);
        }

        [Fact]
        public async Task RegisterCommand_EmailTaken_Failure()
        {
            var userFactory = Substitute.For<IUserFactory>();

            var hashService = Substitute.For<IHashService>();

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(Arg.Any<string>()).Returns(new User());


            var validator = Substitute.For<IValidator<RegisterCommand>>();

            var handler = new RegisterCommandHandler(userRepository, hashService, userFactory, validator);

            var command = new RegisterCommand
            {
                Email = "email"
            };

            var response = await handler.Handle(command, default);

            Assert.False(response.IsSuccess);
            Assert.NotEmpty(response.Error);
        }

        [Fact]
        public async Task RegisterCommand_InvalidRequest_Failure()
        {
            var userFactory = Substitute.For<IUserFactory>();

            var hashService = Substitute.For<IHashService>();

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(Arg.Any<string>()).ReturnsNull();

            var validator = Substitute.For<IValidator<RegisterCommand>>();

            validator.ValidateAsync(Arg.Any<RegisterCommand>()).Returns(
                new ValidationResult(new List<ValidationFailure> { new ValidationFailure("prop", "invalid prop") }));

            var handler = new RegisterCommandHandler(userRepository, hashService, userFactory, validator);

            var command = new RegisterCommand
            {
                Email = "email"
            };

            var response = await handler.Handle(command, default);

            Assert.False(response.IsSuccess);
            Assert.NotEmpty(response.Error);
            Assert.NotEmpty(response.ValidationErrors);
        }
    }
}
