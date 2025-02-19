using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Services
{
    public class DogService : IDogService
    {
        private readonly IDogRepository _dogRepository;

        public DogService(IDogRepository dogRepository)
        {
            _dogRepository = dogRepository;
        }

        public async Task<Dog?> Create(Dog dog, string userId)
        {
            // לעדכן ליוזרת את הכלב הפעיל האחרון
            try
            {
                if (dog == null) return null;
                dog.UserId = userId;
                return await _dogRepository.CreateDog(dog);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Dog>> GetAll()
        {
            try
            {
                return await _dogRepository.GetAllDogs();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Dog>> GetAll(string userId)
        {
            try
            {

                return await _dogRepository.GetAllUserDogs(userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<Dog?> GetOne(string id)
        {
            try
            {
                return await _dogRepository.GetDogById(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<Dog?> Update(string id, Dog dog)
        {
            try
            {
                return await _dogRepository.UpdateDog(id, dog);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<Dog?> Delete(string id)
        {
            try
            {
                return await _dogRepository.DeleteDog(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
