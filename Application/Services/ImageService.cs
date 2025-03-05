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
        private readonly IImageRepository _imageRepository;
        private readonly string _bucketName = "your-bucket-name"; // Replace with your bucket name

        public ImageService(IImageRepository imageRepository)
        {
            _storageClient = StorageClient.Create();
            _imageRepository = imageRepository;
        }

        public async Task<string?> UploadImage(IFormFile file, int entityId, EntityType entityType)
        {
            if (file == null || file.Length == 0) return null;

            List<Image> existingImages = await _imageRepository.GetAllEntityImagesDb(entityId, entityType);

            if (existingImages.Count >= 5) return null;

            string fileName = $"{entityType}_{entityId}_{DateTime.Now.ToString()}";
            using var stream = file.OpenReadStream();

            // file.GetType();

            var obj = await _storageClient.UploadObjectAsync(_bucketName, fileName, file.ContentType, stream); // לבדוק שהצליחה הפעולה
            string fileUrl = $"https://storage.googleapis.com/{_bucketName}/{fileName}";

            Image? newImage = new Image
            {
                EntityId = entityId,
                EntityType = entityType,
                Url = fileUrl,
                Order = existingImages.Count()
            };

            Image? imageToReturn = await _imageRepository.CreateImageDb(newImage);

            if (imageToReturn == null) return null;
            return fileUrl;
        }

        public async Task<string?> RemoveImage(int imageId, int entityId, EntityType entityType)
        {
            Image? imageToDelete = await _imageRepository.GetImageByIdDb(imageId);
            if (imageToDelete == null || imageToDelete.Url == null)
                return null;

            List<Image> entityImagesList = await _imageRepository.GetAllEntityImagesDb(entityId, entityType);

            var uri = new Uri(imageToDelete.Url);
            string fileName = uri.Segments.Last();

            try
            {
                await _storageClient.DeleteObjectAsync(_bucketName, fileName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "GoogleCloud image deleting has been failed";
            }

            if (await _imageRepository.DeleteImageDb(imageId) == null)
                return "Delete action has been failed";

            for (int i = imageToDelete.Order + 1; i < entityImagesList.Count; i++)
            {
                entityImagesList[i].Order--;
                if (await _imageRepository.UpdateImageDb(entityImagesList[i].Id, entityImagesList[i]) == null)
                    return "Order update action has been failed";
            }

            return "Image deleted successfully.";
        }

        public async Task<string?> ReOrderImages(int imageId1, int imageId2)
        {
            Image? imageToReplace1 = await _imageRepository.GetImageByIdDb(imageId1);
            Image? imageToReplace2 = await _imageRepository.GetImageByIdDb(imageId2);
            if (imageToReplace1 == null || imageToReplace2 == null || imageToReplace1.EntityId != imageToReplace2.EntityId)
                return null;

            (imageToReplace1.Order, imageToReplace2.Order) = (imageToReplace2.Order, imageToReplace1.Order);

            if (await _imageRepository.UpdateImageDb(imageToReplace1.Id, imageToReplace1) == null ||
                await _imageRepository.UpdateImageDb(imageToReplace2.Id, imageToReplace2) == null)
                return "ReOrder has been failed";

            return "ReOrder has been changed successfully";
        }
    }
}
