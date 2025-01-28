using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IParkRepository
    {
        public Task<Park?> CreatePark(string id);
        public Task<List<Park?>> GetAllParks(string id);
        public Task<Park?> GetParkById(string id);
        public Task<Park?> UpdatePark(string id);
        public Task<Park?> DeletePark(string id);
        public Task<Park?> LikeUnlikePark(string id);
    }
}
