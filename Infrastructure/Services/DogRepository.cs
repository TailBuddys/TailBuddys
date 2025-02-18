using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task<Dog?> CreateDog(Dog dog)
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
        public async Task<List<Dog>> GetAllDogs()
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
        public async Task<List<Dog>> GetAllUserDogs(string userId)
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
        public async Task<List<Dog>> GetUnMatchedDogs(string dogId)
        {
            try
            {
                List<Dog> list = await _context.Dogs.Where(d => d.MatchesAsFrom.All(m => m.FromDogId != dogId)).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Dog>();
            }
        }
        public async Task<Dog?> GetDogById(string dogId)
        {
            try
            {
                Dog? d = await _context.Dogs.Include(d => d.FavParks).FirstOrDefaultAsync(d => d.Id == dogId);
                if(d == null)
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
        public async Task<Dog?> UpdateDog(string dogId, Dog dog)
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
        public async Task<Dog?> DeleteDog(string dogId)
        {
            try
            {
                Dog? dogToRemove = await _context.Dogs.Include(p => p.FavParks).FirstOrDefaultAsync(p => p.Id == dogId);
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
