using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Services
{
    // בגלל שבמיגרציה מחיקת אובייקט מוגדר כריסטריקט
    // יש לדאוג למחוק את כל תת הישוייות של כלב לפני שמוחקים את הכלב עצמו צ'אטים, מאצ'ים, הודעות וכו
    public class DogService : IDogService
    {
        private readonly IDogRepository _dogRepository;

        public DogService(IDogRepository dogRepository)
        {
            _dogRepository = dogRepository;
        }

        public async Task<Dog?> Create(Dog dog, string userId)
        {
            // לעדכן ליוזר את הכלב הפעיל האחרון
            try
            {
                if (dog == null) return null;
                dog.UserId = userId;
                return await _dogRepository.CreateDogDb(dog);
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
                return await _dogRepository.GetAllDogsDb();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Dog>();
            }
        }
        public async Task<List<Dog>> GetAll(string userId)
        {
            try
            {

                return await _dogRepository.GetAllUserDogsDb(userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Dog>> GetUnmatchedDogs(string dogId)
        {
            try
            {

                return await _dogRepository.GetUnMatchedDogsDb(dogId);
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
                return await _dogRepository.GetDogByIdDb(id);
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
                return await _dogRepository.UpdateDogDb(id, dog);
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
                return await _dogRepository.DeleteDogDb(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
