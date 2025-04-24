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
        private readonly ILogger<ParkService> _logger;


        public ParkService(IParkRepository parkRepository, IImageService imageService, IDogService dogService, ILogger<ParkService> logger)
        {
            _parkRepository = parkRepository;
            _imageService = imageService;
            _dogService = dogService;
            _logger = logger;
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
                _logger.LogError(e, "Error occurred while creating park."); 
                return null;
            }
        }

        private double CalculateDistanceKm(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; 
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
        public async Task<List<ParkDTO>> GetAllParks(int? dogId, [FromQuery] ParksFilterDTO filters)
        {
            try
            {
                var parks = await _parkRepository.GetAllParksDb();

                if (filters.DogLikes != null)
                {
                    parks = parks.Where(p => p.DogLikes.Count >= filters.DogLikes.Value).ToList();
                }

                double? dogLat = null, dogLon = null;

                if (dogId != null)
                {
                    DogDTO? dog = await _dogService.GetOne(dogId.Value, true);
                    if (dog != null)
                    {
                        dogLat = dog.Lat;
                        dogLon = dog.Lon;

                        if (filters.Distance != null && dogLat != 0 && dogLon != 0)
                        {
                            double earthRadiusKm = 6371;
                            double deltaLat = filters.Distance.Value / earthRadiusKm * (180 / Math.PI);
                            double deltaLon = filters.Distance.Value / (earthRadiusKm * Math.Cos((double)dogLat * Math.PI / 180)) * (180 / Math.PI);

                            parks = parks
                                .Where(p =>
                                    p.Lat >= dogLat - deltaLat && p.Lat <= dogLat + deltaLat &&
                                    p.Lon >= dogLon - deltaLon && p.Lon <= dogLon + deltaLon)
                                .ToList();
                        }
                    }
                }

                var finalList = parks
                    .Select(p =>
                    {
                        double? distance = null;
                        if (dogLat != null && dogLon != null)
                        {
                            distance = CalculateDistanceKm(dogLat.Value, dogLon.Value, p.Lat, p.Lon);
                        }

                        return new ParkDTO
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Address = p.Address,
                            Lat = p.Lat,
                            Lon = p.Lon,
                            Distance = distance,
                            DogLikes = p.DogLikes.Select(d => new UserDogDTO
                            {
                                Id = d.Id,
                                Name = d.Name,
                                ImageUrl = d.Images.FirstOrDefault(i => i.Order == 0)?.Url
                            }).ToList(),
                            Images = p.Images.OrderBy(i => i.Order).Select(i => new ImageDTO
                            {
                                Id = i.Id,
                                Url = i.Url
                            }).ToList()
                        };
                    })
                    .Where(p => filters.Distance == null || p.Distance <= filters.Distance)
                    .ToList();

                return dogId != null ? finalList.OrderBy(p => p.Distance).ToList() : finalList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving parks with filters.");
                return new List<ParkDTO>();
            }
        }
        //public async Task<List<ParkDTO>> GetAllParks(int? dogId, [FromQuery] ParksFilterDTO filters)
        //{
        //    try
        //    {
        //        List<EntityDistance> updatedParksDistance = new List<EntityDistance>();
        //        List<Park> parks = await _parkRepository.GetAllParksDb();

        //        if (filters.DogLikes != null)
        //        {
        //            parks = parks.Where(park => park.DogLikes.Count >= filters.DogLikes).ToList();

        //        }

        //        if (dogId != null)
        //        {
        //            DogDTO? myDog = await _dogService.GetOne(dogId.Value, true);
        //            if (myDog != null)
        //            {
        //                EntityDistance myDogLocation = new EntityDistance
        //                {
        //                    EntityId = myDog.Id,
        //                    Lat = myDog.Lat,
        //                    Lon = myDog.Lon,
        //                };

        //                List<EntityDistance> parksDistance = parks
        //                .Select(park => new EntityDistance { EntityId = park.Id, Lat = park.Lat, Lon = park.Lon }).ToList();

        //                updatedParksDistance = MapDistanceHelper.CalculateDistance(myDogLocation, parksDistance);

        //                if (filters.Distance != null)
        //                {
        //                    updatedParksDistance = updatedParksDistance.Where(park => park.Distance < filters.Distance).ToList();
        //                }

        //                parks = parks.Where(park => updatedParksDistance.Any(ed => ed.EntityId == park.Id)).ToList();
        //            }
        //        }

        //        List<ParkDTO> finalParksList = parks
        //            .Select(park => new ParkDTO
        //            {
        //                Id = park.Id,
        //                Name = park.Name,
        //                Description = park.Description,
        //                Address = park.Address,
        //                Lat = park.Lat,
        //                Lon = park.Lon,
        //                DogLikes = park.DogLikes.Select(d => new UserDogDTO
        //                {
        //                    Id = d.Id,
        //                    Name = d.Name,
        //                    ImageUrl = d.Images.FirstOrDefault(i => i.Order == 0)?.Url
        //                }).ToList(),
        //                Images = park.Images.OrderBy(d => d.Order).Select(image => new ImageDTO { Id = image.Id, Url = image.Url }).ToList(),
        //                Distance = updatedParksDistance.FirstOrDefault(ed => ed.EntityId == park.Id)?.Distance
        //            }).ToList();
        //        if (dogId != null)
        //        {
        //            return finalParksList.OrderBy(p => p.Distance).ToList();
        //        }

        //        return finalParksList;
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e, "Error occurred while receiving all parks park."); 
        //        return new List<ParkDTO>();
        //    }
        //}
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
                    DogLikes = park.DogLikes.Select(d => new UserDogDTO
                    {
                        Id = d.Id,
                        Name = d.Name,
                        ImageUrl = d.Images.FirstOrDefault(i => i.Order == 0)?.Url
                    }).ToList(),
                    Images = ParkImages
                };

                return parkToReturn;
            }

            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while getting park by Id"); 
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
                _logger.LogError(e, "Error occurred while updating park."); 
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
                _logger.LogError(e, "Error occurred while deleting park."); 
                return null;
            }
        }
        public async Task<ParkDTO?> LikeUnlikePark(int parkId, int dogId)
        {
            try
            {
                Park? park = await _parkRepository.LikeUnlikeParkDb(parkId, dogId);
                if (park == null) return null;
                return new ParkDTO
                {
                    Id = park.Id,
                    DogLikes = park.DogLikes.Select(d => new UserDogDTO
                    {
                        Id = d.Id,
                        Name = d.Name,
                        ImageUrl = d.Images.FirstOrDefault(i => i.Order == 0)?.Url
                    }).ToList(),
                };
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while like/unlike park."); 
                return null;
            }
        }
    }
}
