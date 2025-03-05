using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Services
{
    public class ParkService : IParkService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IImageService _imageService;

        public ParkService(IParkRepository parkRepository, IImageService imageService)
        {
            _parkRepository = parkRepository;
            _imageService = imageService;
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
        public async Task<Park?> GetParkById(int parkId)
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
        public async Task<Park?> UpdatePark(int parkId, Park park)
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
        public async Task<Park?> DeletePark(int parkId)
        {
            Park? parkToDelete = await _parkRepository.GetParkByIdDb(parkId);
            if (parkToDelete == null) return null;

            try
            {
                List<Image> imagesList = parkToDelete.Images.ToList();

                foreach (Image image in imagesList)
                {
                    await _imageService.RemoveImage(image.Id, parkToDelete.Id, 1);
                }

                return await _parkRepository.DeleteParkDb(parkId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<Park?> LikeUnlikePark(int parkId, int dogId)
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
