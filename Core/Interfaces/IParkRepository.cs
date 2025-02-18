using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IParkRepository
    {
        public Task<Park?> CreatePark(Park park);
        public Task<List<Park>> GetAllParks();
        public Task<Park?> GetParkById(string parkId);
        public Task<Park?> UpdatePark(string parkId, Park park);
        public Task<Park?> DeletePark(string parkId);
        public Task<Park?> LikeUnlikePark(string parkId, Dog dog);
    }
}
