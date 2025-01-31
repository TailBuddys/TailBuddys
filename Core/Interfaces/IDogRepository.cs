using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IDogRepository
    {
        public Task<Dog?> CreateDog(Dog dog);
        public Task<List<Dog>> GetAllDogs();
        public Task<List<Dog>> GetAllUserDogs(string userId);
        // return all new dogs that this dog didn't like/ unlike
        public Task<List<Dog>> GetUnMatchedDogs(string dogId);
        public Task<Dog?> GetDogById(string dogId);
        public Task<Dog?> UpdateDog(string dogId, Dog dog);
        public Task<Dog?> DeleteDog(string dogId);

    }
}
