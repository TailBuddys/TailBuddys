using Microsoft.EntityFrameworkCore;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Infrastructure.Data;

namespace TailBuddys.Infrastructure.Services
{
    public class DogRepository : IDogRepository
    {
        private readonly TailBuddysContext _context;
        public DogRepository(TailBuddysContext context)
        {
            _context = context;
        }
        // החלפה של כלבים
        //public async Task<Dog?> SwitchDog(int currentDogId, int switchDogId)
        public async Task<Dog?> CreateDogDb(Dog dog)
        {
            try
            {
                _context.Dogs.Add(dog);
                await _context.SaveChangesAsync();
                return dog;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
                Console.WriteLine(ex);
                return new List<Dog>();
            }
        }
        public async Task<List<Dog>> GetAllUserDogsDb(int userId)
        {
            try
            {
                return await _context.Dogs.Include(d => d.FavParks).Where(d => d.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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

                List<int> matchedDogIds = myDog.MatchesAsSender.Select(m => m.ReciverDogId).ToList();

                List<Dog> list = await _context.Dogs
                    .Where(db => !matchedDogIds.Contains(db.Id) && db.UserId != myDog.UserId)
                    .ToListAsync();

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
                    .Include(d => d.MatchesAsReciver)
                    .Include(d => d.ChatsAsSender)
                    .Include(d => d.ChatsAsReciver)
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
                Console.WriteLine(ex);
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
                dogToUpdate.Geneder = dog.Geneder;
                dogToUpdate.Birthdate = dog.Birthdate;
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
                Console.WriteLine(ex);
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
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
