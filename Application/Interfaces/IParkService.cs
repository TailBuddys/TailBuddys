using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IParkService
    {
       Task<Park?> CreateParkDb(Park park);
       Task<List<Park>> GetAllParksDb();
       Task<Park?> GetParkByIdDb(string parkId);
       Task<Park?> UpdateParkDb(string parkId, Park park);
       Task<Park?> DeleteParkDb(string parkId);
       Task<Park?> LikeUnlikeParkDb(string parkId, string dogId);
    }
}
