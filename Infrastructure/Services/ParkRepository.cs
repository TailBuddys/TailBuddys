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
        public async Task<Park?> CreatePark(Park park)
        {
            try
            {
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
        public async Task<List<Park>> GetAllParks()
        {
            try
            {
                return await _context.Parks.Include(p => p.DogLikes).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Park>();
            }
        }
        public async Task<Park?> GetParkById(string parkId)
        {
            try
            {
                return await _context.Parks.Include(p => p.DogLikes).FirstOrDefaultAsync(p => p.Id == parkId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<Park?> UpdatePark(string parkId, Park park)
        {
            try
            {
                Park? parkToUpdate = await _context.Parks.FirstOrDefaultAsync(p => p.Id == parkId);
                if (parkToUpdate == null)
                {
                    return null;
                }
                parkToUpdate.Name = park.Name;
                parkToUpdate.Address = park.Address;
                parkToUpdate.Lon = park.Lon;
                parkToUpdate.Lat = park.Lat;
                parkToUpdate.UpdatedDate = DateTime.Now;
      
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
        public async Task<Park?> DeletePark(string parkId)
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
        public async Task<Park?> LikeUnlikePark(string parkId, Dog dog)
        {
            try
            {
                Park? park = await _context.Parks.Include(p => p.DogLikes).FirstOrDefaultAsync(p => p.Id == parkId);
                if (park == null)
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
