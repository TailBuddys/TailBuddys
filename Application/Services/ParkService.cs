using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Infrastructure.Services;

namespace TailBuddys.Application.Services
{
    public class ParkService : IParkService
    {
        private readonly IParkRepository _parkRepository;

        public ParkService(IParkRepository parkRepository)
        {
            _parkRepository = parkRepository;
        }
        //לבדוק שמי שמנסה להקים פארק הוא מנהל 
        //כנ"ל לגבי עריכה + מחיקה של פארק
        public async Task<Park?> CreateParkDb(Park park)
        {
            try
            {
                if (park == null) return null;
                return await _parkRepository.CreateParkDb(park);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Park>> GetAllParksDb()
            {
            try
            {
                return await _parkRepository.GetAllParksDb();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Park>();
            }

        }
        public async Task<Park?> GetParkByIdDb(string parkId)
        {
            try
            {
                return await _parkRepository.GetParkByIdDb(parkId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }                 
        public async Task<Park?> UpdateParkDb(string parkId, Park park)
            {
            try
            {
                return await _parkRepository.UpdateParkDb(parkId, park);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<Park?> DeleteParkDb(string parkId)
            {
            try
            {
                return await _parkRepository.DeleteParkDb(parkId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<Park?> LikeUnlikeParkDb(string parkId, string dogId)
            {
            try
            {
                return await _parkRepository.LikeUnlikeParkDb(parkId, dogId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
              }
    }
}
