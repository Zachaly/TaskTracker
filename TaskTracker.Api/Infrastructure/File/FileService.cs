namespace TaskTracker.Api.Infrastructure
{
    public interface IFileService
    {
        Task<FileStream> GetProfilePictureAsync(string fileName);
        Task<string> SaveProfilePictureAsync(IFormFile file);
        Task DeleteProfilePictureAsync(string fileName);
    }

    public class FileService : IFileService
    {
        private readonly string _profilePicturePath;

        public FileService(IConfiguration configuration)
        {
            _profilePicturePath = configuration["Image:ProfilePicturePath"]!;
        }

        public Task DeleteProfilePictureAsync(string fileName)
        {
            var path = Path.Combine(_profilePicturePath, fileName);

            File.Delete(path);

            return Task.CompletedTask;
        }

        public Task<FileStream> GetProfilePictureAsync(string fileName)
            => Task.FromResult(File.OpenRead(Path.Combine(_profilePicturePath, fileName)));

        public async Task<string> SaveProfilePictureAsync(IFormFile file)
        {
            Directory.CreateDirectory(_profilePicturePath);

            var fileName = $"{Guid.NewGuid()}.png";

            var filePath = Path.Combine(_profilePicturePath, fileName);

            using var stream = File.Create(filePath);

            await file.CopyToAsync(stream);

            return fileName;
        }
    }
}
