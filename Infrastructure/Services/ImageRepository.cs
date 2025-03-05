using Microsoft.EntityFrameworkCore;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Infrastructure.Data;

namespace TailBuddys.Infrastructure.Services
{
    public class ImageRepository : IImageRepository
    {
        private readonly TailBuddysContext _context;
        public ImageRepository(TailBuddysContext context)
        {
            _context = context;
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
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<List<Image>> GetAllEntityImagesDb(int entityId, EntityType entityType)
        {
            try
            {
                return await _context.Images
                    .Where(i => i.EntityId == entityId && i.EntityType == entityType)
                    .OrderBy(i => i.Order)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
                Console.WriteLine(ex);
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
                Console.WriteLine(ex);
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
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}

