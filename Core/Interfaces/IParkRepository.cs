using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IParkRepository
    {
        public Task<Park?> CreateParkDb(Park park);
        public Task<List<Park>> GetAllParksDb();
        public Task<Park?> GetParkByIdDb(string parkId);
        public Task<Park?> UpdateParkDb(string parkId, Park park);
        public Task<Park?> DeleteParkDb(string parkId);
        public Task<Park?> LikeUnlikeParkDb(string parkId, Dog dog);
    }
}
