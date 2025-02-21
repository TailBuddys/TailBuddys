using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;

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
        public async Task<Park?> CreatePark(Park park)
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
        public async Task<List<Park>> GetAllParks()
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
        public async Task<Park?> GetParkById(string parkId)
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
        public async Task<Park?> UpdatePark(string parkId, Park park)
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
        public async Task<Park?> DeletePark(string parkId)
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
        public async Task<Park?> LikeUnlikePark(string parkId, string dogId)
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
