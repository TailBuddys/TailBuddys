using TailBuddys.Core.DTO;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.DTO;

namespace TailBuddys.Application.Interfaces
{
    public interface IParkService
    {
        Task<Park?> CreatePark(Park park);
        Task<List<ParkDTO>> GetAllParks(int? dogId, ParksFilterDTO filters);
        Task<ParkDTO?> GetParkById(int parkId);
        Task<Park?> UpdatePark(int parkId, Park park);
        Task<Park?> DeletePark(int parkId);
        Task<ParkDTO?> LikeUnlikePark(int parkId, int dogId);
    }
}
