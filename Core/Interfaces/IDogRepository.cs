using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IDogRepository
    {
        public Task<Dog?> CreateDogDb(Dog dog);
        public Task<List<Dog>> GetAllDogsDb();
        public Task<List<Dog>> GetAllUserDogsDb(string userId);
        // return all new dogs that this dog didn't like/ unlike
        public Task<List<Dog>> GetUnMatchedDogsDb(string dogId);
        public Task<Dog?> GetDogByIdDb(string dogId);
        public Task<Dog?> UpdateDogDb(string dogId, Dog dog);
        public Task<Dog?> DeleteDogDb(string dogId);

    }
}
