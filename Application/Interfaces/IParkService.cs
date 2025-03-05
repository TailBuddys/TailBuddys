using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IParkService
    {
        Task<Park?> CreatePark(Park park);
        Task<List<Park>> GetAllParks();
        Task<Park?> GetParkById(int parkId);
        Task<Park?> UpdatePark(int parkId, Park park);
        Task<Park?> DeletePark(int parkId);
        Task<Park?> LikeUnlikePark(int parkId, int dogId);
    }
}
