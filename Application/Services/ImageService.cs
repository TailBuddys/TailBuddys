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
        private readonly string _bucketName = "tail_buddys_bucket1"; // להסתיר במשתנה סביבה

        public ImageService(IImageRepository imageRepository)
        {
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "C:\\tailbuddys-570e8d8b9cdd.json");
            _storageClient = StorageClient.Create();
            _imageRepository = imageRepository;
        }

        public async Task<string?> UploadImage(IFormFile file, int entityId, int? entityType)
        {

            if (file == null || file.Length == 0) return "file not good";

            List<Image> existingImages = await _imageRepository.GetAllEntityImagesDb(entityId, entityType);

            if (existingImages.Count >= 5) return "gadol me 5";

            string fileName = $"{entityType}_{entityId}_{Guid.NewGuid()}";
            using var stream = file.OpenReadStream();

            // file.GetType();

            var obj = await _storageClient.UploadObjectAsync(_bucketName, fileName, file.ContentType, stream); // לבדוק שהצליחה הפעולה
            if (obj == null || string.IsNullOrEmpty(obj.MediaLink)) return null;

            string fileUrl = $"https://storage.googleapis.com/{_bucketName}/{fileName}";


            Image? newImage = new Image
            {
                Url = fileUrl,
                Order = _imageRepository.GetAllEntityImagesDb(entityId, entityType).Result.Count()
            };

            if (entityType == 0)
            {
                newImage.DogId = entityId;
            }
            else
            {
                newImage.ParkId = entityId;
            }

            Image? imageToReturn = await _imageRepository.CreateImageDb(newImage);

            if (imageToReturn == null) return "repos fail";
            return fileUrl;
        }

        public async Task<string?> RemoveImage(int imageId, int entityId, int? entityType)
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
            if (imageToReplace1 == null || imageToReplace2 == null)
                return null;

            if ((imageToReplace1.DogId == imageToReplace2.DogId && imageToReplace1.DogId != null) ||
               (imageToReplace1.ParkId == imageToReplace2.ParkId && imageToReplace1.ParkId != null))
            {
                (imageToReplace1.Order, imageToReplace2.Order) = (imageToReplace2.Order, imageToReplace1.Order);

                if (await _imageRepository.UpdateImageDb(imageToReplace1.Id, imageToReplace1) == null ||
                    await _imageRepository.UpdateImageDb(imageToReplace2.Id, imageToReplace2) == null)
                    return "ReOrder has been failed";

                return "ReOrder has been changed successfully";
            }
            return "Invallid action";
        }

        public async Task<bool> IsFirstImage(int dogId)
        {
            List<Image> dogImages = await _imageRepository.GetAllEntityImagesDb(dogId, 0);
            if (dogImages.Count == 1)
            {
                return true;
            }
            return false;
        }
    }
}
