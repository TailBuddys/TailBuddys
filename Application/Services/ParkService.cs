using Microsoft.AspNetCore.Mvc;
using TailBuddys.Application.Interfaces;
using TailBuddys.Application.Utils;
using TailBuddys.Core.DTO;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.DTO;
using TailBuddys.Core.Models.SubModels;

namespace TailBuddys.Application.Services
{
    public class ParkService : IParkService
    {
        private readonly IParkRepository _parkRepository;
        private readonly IImageService _imageService;
        private readonly IDogService _dogService;

        public ParkService(IParkRepository parkRepository, IImageService imageService, IDogService dogService)
        {
            _parkRepository = parkRepository;
            _imageService = imageService;
            _dogService = dogService;
        }
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
        // להחזיר DTO של פארקים
        // לחשב מרחקים בין כלב אל פארקים סובבים?
        // במידה ומשתמש לא הגדיר לעצמו נק' ציון (מיקום) להחזיר רשימה של פארקים ע"פ דרגת דירוג
        // לקבל פרמטר סינון
        public async Task<List<ParkDTO>> GetAllParks(int? dogId, [FromQuery] ParksFilterDTO filters)
        {
            try
            {
                List<EntityDistance> updatedParksDistance = new List<EntityDistance>();
                List<Park> parks = await _parkRepository.GetAllParksDb();

                if (filters.DogLikes != null)
                {
                    parks = parks.Where(park => park.DogLikes.Count > filters.DogLikes).ToList();
                }

                if (dogId != null)
                {
                    DogDTO? myDog = await _dogService.GetOne(dogId.Value, true);
                    EntityDistance myDogLocation = new EntityDistance
                    {
                        EntityId = myDog.Id,
                        Lat = myDog.Lat,
                        Lon = myDog.Lon,
                    };

                    List<EntityDistance> parksDistance = parks
                    .Select(park => new EntityDistance { EntityId = park.Id, Lat = park.Lat, Lon = park.Lon }).ToList();

                    updatedParksDistance = MapDistanceHelper.CalculateDistance(myDogLocation, parksDistance);

                    if (filters.Distance != null)
                    {
                        updatedParksDistance = updatedParksDistance.Where(park => park.Distance < filters.Distance).ToList();
                    }

                    parks = parks.Where(park => updatedParksDistance.Any(ed => ed.EntityId == park.Id)).ToList();
                }
                List<ParkDTO> finalParksList = parks
                    .Select(park => new ParkDTO
                    {
                        Id = park.Id,
                        Name = park.Name,
                        Description = park.Description,
                        Address = park.Address,
                        DogLikes = park.DogLikes.Count(),
                        Images = park.Images.OrderBy(d => d.Order).Select(image => new ImageDTO { Id = image.Id, Url = image.Url }).ToList(),
                        Distance = updatedParksDistance.FirstOrDefault(ed => ed.EntityId == park.Id)?.Distance
                    }).ToList();

                return finalParksList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<ParkDTO>();
            }
        }
        public async Task<ParkDTO?> GetParkById(int parkId)
        {
            try
            {
                Park? park = await _parkRepository.GetParkByIdDb(parkId);
                if (park == null)
                {
                    return null;
                }
                List<ImageDTO> ParkImages = new List<ImageDTO>();
                foreach (Image image in park.Images.OrderBy(d => d.Order))
                {
                    ParkImages.Add(new ImageDTO
                    {
                        Id = image.Id,
                        Url = image.Url
                    });
                }
                ParkDTO parkToReturn = new ParkDTO
                {
                    Id = park.Id,
                    Name = park.Name,
                    Description = park.Description,
                    Address = park.Address,
                    Lon = park.Lon,
                    Lat = park.Lat,
                    DogLikes = park.DogLikes.Count,
                    Images = ParkImages
                };

                return parkToReturn;
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
