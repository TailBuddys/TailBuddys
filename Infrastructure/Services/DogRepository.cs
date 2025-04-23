using Microsoft.EntityFrameworkCore;
using TailBuddys.Application.Services;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Infrastructure.Data;

namespace TailBuddys.Infrastructure.Services
{
    public class DogRepository : IDogRepository
    {
        private readonly TailBuddysContext _context;
        private readonly ILogger<DogRepository> _logger;

        public DogRepository(TailBuddysContext context, ILogger<DogRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        // החלפה של כלבים
        //public async Task<Dog?> SwitchDog(int currentDogId, int switchDogId)
        public async Task<Dog?> CreateDogDb(Dog dog)
        {
            try
            {
                dog.CreatedAt = DateTime.Now;
                dog.UpdatedAt = DateTime.Now;
                _context.Dogs.Add(dog);
                await _context.SaveChangesAsync();
                return dog;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating new dog.");
                return null;
            }
        }
        public async Task<List<Dog>> GetAllDogsDb()
        {
            try
            {
                return await _context.Dogs.Include(d => d.FavParks).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all dog.");
                return new List<Dog>();
            }
        }
        public async Task<List<Dog>> GetAllUserDogsDb(int userId)
        {
            try
            {
                return await _context.Dogs
                    .Include(d => d.Images)
                    .Include(d => d.FavParks)
                    .Where(d => d.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all user dogs.");
                return new List<Dog>();
            }
        }
        public async Task<List<Dog>> GetUnMatchedDogsDb(int dogId)
        {
            try
            {
                Dog? myDog = await _context.Dogs
                    .Include(d => d.MatchesAsSender)
                    .FirstOrDefaultAsync(db => db.Id == dogId);

                if (myDog == null)
                    return new List<Dog>();

                List<int> matchedDogIds = myDog.MatchesAsSender.Select(m => m.ReceiverDogId).ToList();

                List<Dog> list = await _context.Dogs
                    .Include(d => d.Images.OrderBy(i => i.Order))
                    .Where(db => !matchedDogIds.Contains(db.Id) && db.UserId != myDog.UserId)
                    .ToListAsync();

                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting unmatched dogs.");
                return new List<Dog>();
            }
        }
        public async Task<Dog?> GetDogByIdDb(int dogId)
        {
            try
            {
                Dog? d = await _context.Dogs
                    .Include(d => d.FavParks)
                    .Include(d => d.MatchesAsSender)
                    .Include(d => d.MatchesAsReceiver)
                    .Include(d => d.ChatsAsSender)
                    .Include(d => d.ChatsAsReceiver)
                    .Include(d => d.Images)
                    .FirstOrDefaultAsync(d => d.Id == dogId);

                if (d == null)
                {
                    return null;
                }
                return d;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting dog by id.");
                return null;
            }
        }
        public async Task<Dog?> UpdateDogDb(int dogId, Dog dog)
        {
            try
            {
                Dog? dogToUpdate = await _context.Dogs.FirstOrDefaultAsync(d => d.Id == dogId);
                if (dogToUpdate == null)
                {
                    return null;
                }
                dogToUpdate.Name = dog.Name;
                dogToUpdate.Description = dog.Description;
                dogToUpdate.Type = dog.Type;
                dogToUpdate.Size = dog.Size;
                dogToUpdate.Gender = dog.Gender;
                dogToUpdate.BirthDate = dog.BirthDate;
                dogToUpdate.Address = dog.Address;
                dogToUpdate.Lon = dog.Lon;
                dogToUpdate.Lat = dog.Lat;
                dogToUpdate.Vaccinated = dog.Vaccinated;
                dogToUpdate.UpdatedAt = DateTime.Now;
                dogToUpdate.Images = dog.Images;

                _context.Dogs.Update(dogToUpdate);
                await _context.SaveChangesAsync();
                return dogToUpdate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating dog.");
                return null;
            }
        }
        public async Task<Dog?> DeleteDogDb(int dogId)
        {
            try
            {
                Dog? dogToRemove = await _context.Dogs
                    .Include(p => p.FavParks)
                    .FirstOrDefaultAsync(p => p.Id == dogId);

                if (dogToRemove == null)
                {
                    return null;
                }

                foreach (Park park in dogToRemove.FavParks.ToList())
                {
                    park.DogLikes.Remove(dogToRemove);
                }


                dogToRemove.FavParks.Clear();

                _context.Dogs.Remove(dogToRemove);
                await _context.SaveChangesAsync();

                return dogToRemove;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting dog.");
                return null;
            }
        }
    }
}
