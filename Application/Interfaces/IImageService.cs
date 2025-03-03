using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IImageService
    {
        public Task<string> UploadImage(IFormFile file, string entityId, EntityType entityType);
        public Task<string> RemoveImage(int imageId);
        public Task<List<Image>> ReOrderImages(List<Image> newImgOrder);
    }
}
