using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IImageRepository
    {
        public Task<Image?> CreateImageDb(Image image);
        public Task<List<Image>> GetAllEntityImagesDb(string entityId, EntityType entityType);
        public Task<Image?> GetImageByIdDb(int imageId);
        public Task<Image?> UpdateImageDb(int imageId, Image newImage);
        public Task<Image?> DeleteImageDb(int imageId);
    }
}
