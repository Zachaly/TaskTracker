namespace TaskTracker.Api.Infrastructure
{
    public interface IFileService
    {
        Task<FileStream> GetProfilePictureAsync(string fileName);
        Task<string> SaveProfilePictureAsync(IFormFile file);
        Task DeleteProfilePictureAsync(string fileName);

        Task<IEnumerable<string>> SaveTaskFileAttachmentsAsync(params IFormFile[] files);
        Task<FileStream> GetTaskFileAttachmentAsync(string fileName);
        Task DeleteTaskFileAttachmentAsync(string fileName);
    }

    public class FileService : IFileService
    {
        private readonly string _profilePicturePath;
        private readonly string _taskFileAttachmentPath;

        public FileService(IConfiguration configuration)
        {
            _profilePicturePath = configuration["Image:ProfilePicturePath"]!;
            _taskFileAttachmentPath = configuration["TaskAttachmentPath"]!;
        }

        public Task DeleteProfilePictureAsync(string fileName)
        {
            var path = Path.Combine(_profilePicturePath, fileName);

            File.Delete(path);

            return Task.CompletedTask;
        }

        public Task DeleteTaskFileAttachmentAsync(string fileName)
        {
            var path = Path.Combine(_taskFileAttachmentPath, fileName);

            File.Delete(path);

            return Task.CompletedTask;
        }

        public Task<FileStream> GetProfilePictureAsync(string fileName)
            => Task.FromResult(File.OpenRead(Path.Combine(_profilePicturePath, fileName)));

        public Task<FileStream> GetTaskFileAttachmentAsync(string fileName)
            => Task.FromResult(File.OpenRead(Path.Combine(_taskFileAttachmentPath, fileName)));

        public async Task<string> SaveProfilePictureAsync(IFormFile file)
        {
            Directory.CreateDirectory(_profilePicturePath);

            var fileName = $"{Guid.NewGuid()}.png";

            var filePath = Path.Combine(_profilePicturePath, fileName);

            using var stream = File.Create(filePath);

            await file.CopyToAsync(stream);

            return fileName;
        }

        public async Task<IEnumerable<string>> SaveTaskFileAttachmentsAsync(params IFormFile[] files)
        {
            Directory.CreateDirectory(_taskFileAttachmentPath);

            var names = new List<string>();

            foreach(var file in files)
            {
                var fileName = $"{Guid.NewGuid()}.{file.FileName[file.FileName.LastIndexOf('.')..]}";

                var filePath = Path.Combine(_taskFileAttachmentPath, fileName);
    
                using var stream = File.Create(filePath);

                await file.CopyToAsync(stream);

                names.Add(fileName);
            }

            

            return names;
        }
    }
}
