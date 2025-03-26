using TailBuddys.Core.DTO;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.DTO;

namespace TailBuddys.Application.Interfaces
{
    public interface IDogService
    {
        Task<DogDTO?> Create(Dog dog, int userId);
        Task<List<Dog>> GetAll();
        Task<List<UserDogDTO>> GetAll(int userId);
        Task<List<DogDTO>> GetUnmatchedDogs(int dogId, DogsFilterDTO filters);
        Task<Dog?> GetOne(int id);
        Task<Dog?> Update(int id, Dog dog);
        Task<DogDTO?> Delete(int id);
    }
}
