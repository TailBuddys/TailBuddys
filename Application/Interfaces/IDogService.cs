using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IDogService
    {
        Task<Dog?> Create(Dog dog, string userId);
        Task<List<Dog>> GetAll();
        Task<List<Dog>> GetAll(string userId);
        Task<List<Dog>> GetUnmatchedDogs(string dogId);
        Task<Dog?> GetOne(string id);
        Task<Dog?> Update(string id, Dog dog);
        Task<Dog?> Delete(string id);
    }
}
