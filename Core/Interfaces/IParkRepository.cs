using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IParkRepository
    {
        public Task<Park?> CreateParkAsync(string id);
        public Task<List<Park?>> GetAllParksAsync(string id);
        public Task<Park?> GetParkByIdAsync(string id);
        public Task<Park?> UpdateParkAsync(string id);
        public Task<Park?> DeleteParkAsync(string id);
        public Task<Park?> LikeUnlikeParkAsync(string id);
    }
}
