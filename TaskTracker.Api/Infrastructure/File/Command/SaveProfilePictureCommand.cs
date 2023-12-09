using MediatR;
using System.Collections.Immutable;
using TaskTracker.Database.Repository;
using TaskTracker.Model.Response;

namespace TaskTracker.Api.Infrastructure.Command
{
    public class SaveProfilePictureCommand : IRequest<ResponseModel>
    {
        public long UserId { get; set; }
        public IFormFile? File { get; set; }
    }

    public class SaveProfilePictureHandler : IRequestHandler<SaveProfilePictureCommand, ResponseModel>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;

        private readonly string[] _validExtensions;

        public SaveProfilePictureHandler(IUserRepository userRepository, IFileService fileService, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _fileService = fileService;
            _validExtensions = configuration.GetSection("Image:ValidExtensions").Get<string[]>()!;
        }

        public async Task<ResponseModel> Handle(SaveProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var fileExtension = request.File?.FileName.Split(".")[1];

            if (request.File is not null && !_validExtensions.Contains(fileExtension))
            {
                return new ResponseModel("File has invalid extension!");
            }

            var user = await _userRepository.GetByIdAsync(request.UserId, x => x);

            if(user is null)
            {
                return new ResponseModel("User not found!");
            }

            var oldProfilePicture = user.ProfilePicture;

            string? newProfilePicture = null;

            if(request.File is not null)
            {
                newProfilePicture = await _fileService.SaveProfilePictureAsync(request.File);
            }
            
            user.ProfilePicture = newProfilePicture;

            await _userRepository.UpdateAsync(user);

            if(oldProfilePicture is not null)
            {
                await _fileService.DeleteProfilePictureAsync(oldProfilePicture);
            }

            return new ResponseModel();
        }
    }
}
