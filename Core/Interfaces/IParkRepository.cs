using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IParkRepository
    {
        public Task<Park?> CreateParkDb(Park park);
        public Task<List<Park>> GetAllParksDb();
        public Task<Park?> GetParkByIdDb(int parkId);
        public Task<Park?> UpdateParkDb(int parkId, Park park);
        public Task<Park?> DeleteParkDb(int parkId);
        public Task<Park?> LikeUnlikeParkDb(int parkId, int dogId);
    }
}
