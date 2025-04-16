using Microsoft.EntityFrameworkCore;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Infrastructure.Data;

namespace TailBuddys.Infrastructure.Services
{
    public class ParkRepository : IParkRepository
    {
        private readonly TailBuddysContext _context;
        public ParkRepository(TailBuddysContext context)
        {
            _context = context;
        }
        public async Task<Park?> CreateParkDb(Park park)
        {
            try
            {
                park.CreatedAt = DateTime.Now;
                park.UpdatedAt = DateTime.Now;
                _context.Parks.Add(park);
                await _context.SaveChangesAsync();
                return park;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<List<Park>> GetAllParksDb()
        {
            try
            {
                return await _context.Parks
                    .Include(p => p.Images.OrderBy(i => i.Order))
                    .Include(p => p.DogLikes)
                    .ThenInclude(d => d.Images)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Park>();
            }
        }
        public async Task<Park?> GetParkByIdDb(int parkId)
        {
            try
            {
                return await _context.Parks
                    .Include(p => p.DogLikes)
                    .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == parkId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<Park?> UpdateParkDb(int parkId, Park park)
        {
            try
            {
                Park? parkToUpdate = await _context.Parks.FirstOrDefaultAsync(p => p.Id == parkId);
                if (parkToUpdate == null)
                {
                    return null;
                }
                parkToUpdate.Name = park.Name;
                parkToUpdate.Description = park.Description;
                parkToUpdate.Address = park.Address;
                parkToUpdate.Lon = park.Lon;
                parkToUpdate.Lat = park.Lat;
                parkToUpdate.UpdatedAt = DateTime.Now;

                _context.Parks.Update(parkToUpdate);
                await _context.SaveChangesAsync();
                return parkToUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<Park?> DeleteParkDb(int parkId)
        {
            try
            {
                Park? parkToRemove = await _context.Parks.Include(p => p.DogLikes).FirstOrDefaultAsync(p => p.Id == parkId);
                if (parkToRemove == null)
                {
                    return null;
                }

                foreach (Dog dog in parkToRemove.DogLikes.ToList())
                {
                    dog.FavParks.Remove(parkToRemove);
                }

                parkToRemove.DogLikes.Clear();

                _context.Parks.Remove(parkToRemove);
                await _context.SaveChangesAsync();

                return parkToRemove;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<Park?> LikeUnlikeParkDb(int parkId, int dogId)
        {
            try
            {
                Park? park = await _context.Parks.Include(p => p.DogLikes).ThenInclude(d => d.Images).FirstOrDefaultAsync(p => p.Id == parkId);
                Dog? dog = await _context.Dogs.Include(d => d.FavParks).FirstOrDefaultAsync(d => d.Id == dogId);
                if (park == null || dog == null)
                    return null;

                if (park.DogLikes.Any(d => d.Id == dog.Id))
                {
                    park.DogLikes.Remove(dog);
                    dog.FavParks.Remove(park);
                }
                else
                {
                    park.DogLikes.Add(dog);
                    dog.FavParks.Add(park);
                }

                await _context.SaveChangesAsync();
                park = await _context.Parks.Include(p => p.DogLikes).ThenInclude(d => d.Images).FirstOrDefaultAsync(p => p.Id == parkId);
                return park;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
