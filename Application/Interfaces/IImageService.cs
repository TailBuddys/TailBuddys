namespace TailBuddys.Application.Interfaces
{
    public interface IImageService
    {
        public Task<string?> UploadImage(IFormFile file, int entityId, int? entityType);
        public Task<string?> RemoveImage(int imageId, int entityId, int? entityType);
        public Task<string?> ReOrderImages(int imageId1, int imageId2);
    }
}
