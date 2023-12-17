using MediatR;
using TaskTracker.Database.Repository;

namespace TaskTracker.Api.Infrastructure.Command
{
    public class GetProfilePictureQuery : IRequest<FileStream>
    {
        public long UserId { get; set; }
    }

    public class GetProfilePictureHandler : IRequestHandler<GetProfilePictureQuery, FileStream>
    {
        private readonly string _defaultPicture;
        private readonly IUserRepository _userRepository;
        private readonly IFileService _fileService;

        public GetProfilePictureHandler(IUserRepository userRepository, IFileService fileService, IConfiguration configuration)
        {
            _defaultPicture = configuration["Image:DefaultName"]!;
            _userRepository = userRepository;
            _fileService = fileService;
        }

        public async Task<FileStream> Handle(GetProfilePictureQuery request, CancellationToken cancellationToken)
        {
            var fileName = await _userRepository.GetByIdAsync(request.UserId, x => x.ProfilePicture);

            return await _fileService.GetProfilePictureAsync(fileName ?? _defaultPicture);
        }
    }
}
