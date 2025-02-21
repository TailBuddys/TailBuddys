using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IParkService
    {
        Task<Park?> CreatePark(Park park);
        Task<List<Park>> GetAllParks();
        Task<Park?> GetParkById(string parkId);
        Task<Park?> UpdatePark(string parkId, Park park);
        Task<Park?> DeletePark(string parkId);
        Task<Park?> LikeUnlikePark(string parkId, string dogId);
    }
}
