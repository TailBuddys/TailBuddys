using Microsoft.EntityFrameworkCore;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Infrastructure.Data;

namespace TailBuddys.Infrastructure.Services
{
    public class ImageRepository : IImageRepository
    {
        private readonly TailBuddysContext _context;
        private readonly ILogger<ImageRepository> _logger;

        public ImageRepository(TailBuddysContext context, ILogger<ImageRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Image?> CreateImageDb(Image image)
        {
            try
            {
                _context.Images.Add(image);
                await _context.SaveChangesAsync();
                return image;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating new image.");
                return null;
            }
        }
        public async Task<List<Image>> GetAllEntityImagesDb(int entityId, int? entityType)
        {
            try
            {
                string propertyName = entityType == 0 ? "DogId" : "ParkId";

                return await _context.Images
                    .Where(i => EF.Property<int?>(i, propertyName) == entityId)
                    .OrderBy(i => i.Order)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all images.");
                return new List<Image>();
            }
        }
        public async Task<Image?> GetImageByIdDb(int imageId)
        {
            try
            {
                return await _context.Images.FirstOrDefaultAsync(i => i.Id == imageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting image by id.");
                return null;
            }
        }
        public async Task<Image?> UpdateImageDb(int imageId, Image newImage)
        {
            try
            {
                Image? imageToUpdate = await _context.Images.FirstOrDefaultAsync(i => i.Id == imageId);
                if (imageToUpdate == null)
                {
                    return null;
                }
                imageToUpdate.Order = newImage.Order;


                _context.Images.Update(imageToUpdate);
                await _context.SaveChangesAsync();
                return imageToUpdate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating image.");
                return null;
            }
        }
        public async Task<Image?> DeleteImageDb(int imageId)
        {
            try
            {
                Image? imageToRemove = await _context.Images.FirstOrDefaultAsync(p => p.Id == imageId);
                if (imageToRemove == null)
                {
                    return null;
                }

                _context.Images.Remove(imageToRemove);
                await _context.SaveChangesAsync();

                return imageToRemove;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting image.");
                return null;
            }
        }
    }
}

