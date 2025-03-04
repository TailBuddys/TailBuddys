using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IImageService
    {
        public Task<string?> UploadImage(IFormFile file, string entityId, EntityType entityType);
        public Task<string?> RemoveImage(int imageId, string entityId, EntityType entityType);
        public Task<string?> ReOrderImages(int imageId1, int imageId2);
    }
}
