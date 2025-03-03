using Google.Cloud.Storage.V1;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Services
{
    public class ImageService : IImageService
    {
        // אחראי על דיבור עם ענן התמונות, שמירה של תמונה בענן יצירה של כתובת היו.אר.אל לתמונה
        // פניה לרפוזטורי של פארק / כלב בהתאם ועדכון שלו בדאטה בייס
        // להוסיף העלאת תמונה לכלב

        private readonly StorageClient _storageClient;
        private readonly IDogRepository _dogRepository;
        private readonly IParkRepository _parkRepository;
        private readonly string _bucketName = "your-bucket-name"; // Replace with your bucket name

        public ImageService(IDogRepository dogRepository, IParkRepository parkRepository)
        {
            _storageClient = StorageClient.Create();
            _dogRepository = dogRepository;
            _parkRepository = parkRepository;
        }

        public async Task<string> UploadImage(IFormFile file, string entityId, EntityType entityType)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file.");

            string fileName = $"{entityType}_{entityId}_{DateTime.Now.ToString()}";
            using var stream = file.OpenReadStream();

            var obj = await _storageClient.UploadObjectAsync(_bucketName, fileName, file.ContentType, stream);
            string fileUrl = $"https://storage.googleapis.com/{_bucketName}/{fileName}";

            switch (entityType)
            {
                case EntityType.Dog:
                    // Handle Dog case
                    break;

                case EntityType.Park:
                    // Handle Park case
                    break;

                default:
                    throw new ArgumentException("Invalid entity type.");
            }

            return fileUrl;
        }

        public async Task<string> RemoveImage(int imageId, string entityId, EntityType entityType)
        {
            var image = await _dogRepository.GetByIdAsync(imageId);
            if (image == null)
                throw new ArgumentException("Image not found.");

            // Extract file name from the URL
            var uri = new Uri(image.Url);
            string fileName = uri.Segments.Last();

            await _storageClient.DeleteObjectAsync(_bucketName, fileName);
            await _dogRepository.DeleteAsync(imageId);

            return "Image deleted successfully.";
        }
    }
}
