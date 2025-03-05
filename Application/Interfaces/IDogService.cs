using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IDogService
    {
        Task<Dog?> Create(Dog dog, int userId);
        Task<List<Dog>> GetAll();
        Task<List<Dog>> GetAll(int userId);
        Task<List<Dog>> GetUnmatchedDogs(int dogId);
        Task<Dog?> GetOne(int id);
        Task<Dog?> Update(int id, Dog dog);
        Task<Dog?> Delete(int id);
    }
}
