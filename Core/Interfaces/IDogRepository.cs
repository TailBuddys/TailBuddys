using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IDogRepository
    {
        public Task<Dog?> CreateDogDb(Dog dog);
        public Task<List<Dog>> GetAllDogsDb();
        public Task<List<Dog>> GetAllUserDogsDb(int userId);
        public Task<List<Dog>> GetUnMatchedDogsDb(int dogId);
        public Task<Dog?> GetDogByIdDb(int dogId);
        public Task<Dog?> UpdateDogDb(int dogId, Dog dog);
        public Task<Dog?> DeleteDogDb(int dogId);

    }
}
