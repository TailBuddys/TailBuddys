﻿using System;
using TailBuddys.Application.Interfaces;
using TailBuddys.Application.Utils;
using TailBuddys.Core.DTO;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.DTO;
using TailBuddys.Core.Models.SubModels;
using TailBuddys.Infrastructure.Services;

namespace TailBuddys.Application.Services
{
    public class DogService : IDogService
    {
        private readonly IDogRepository _dogRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IImageService _imageService;
        private readonly IUserRepository _userRepository;
        private readonly IAuth _jwtService;
        private readonly ILogger<DogService> _logger;


        public DogService(
            IDogRepository dogRepository,
            IMatchRepository matchRepository,
            IChatRepository chatRepository,
            IImageService imageService,
            IUserRepository userRepository,
            IAuth jwtService, ILogger<DogService> logger)
        {
            _dogRepository = dogRepository;
            _matchRepository = matchRepository;
            _chatRepository = chatRepository;
            _imageService = imageService;
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<DogDTO?> Create(Dog dog, int userId)
        {
            try
            {
                if (dog == null) return null;
                dog.UserId = userId;
                User? user = await _userRepository.GetUserByIdDb(userId);
                if (user == null) return null;
                dog.IsBot = false;

                Dog? dogToCreate = await _dogRepository.CreateDogDb(dog);
                if (dogToCreate == null) return null;


                string? refreshToken = _jwtService.GenerateToken(user);
                if (refreshToken == null) return null;
                List<ImageDTO> dogImages = new List<ImageDTO>();
                foreach (Image image in dogToCreate.Images.OrderBy(d => d.Order))
                {
                    dogImages.Add(new ImageDTO
                    {
                        Id = image.Id,
                        Url = image.Url
                    });
                }
                DogDTO dogToReturn = new DogDTO
                {
                    Id = dogToCreate.Id,
                    Name = dogToCreate.Name,
                    Description = dogToCreate.Description,
                    Type = dogToCreate.Type,
                    Size = dogToCreate.Size,
                    Gender = dogToCreate.Gender,
                    BirthDate = dogToCreate.BirthDate,
                    Images = dogImages,
                    Vaccinated = dogToCreate.Vaccinated,
                    RefreshToken = refreshToken
                };
                if (dogToReturn == null) return null;
                _logger.LogInformation("Successfully create dog {dogToReturn.Id}", dogToReturn.Id);

                return dogToReturn;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred creating new dog.");
                return null;
            }
        }
        public async Task<List<Dog>> GetAll()
        {
            try
            {
                return await _dogRepository.GetAllDogsDb();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred receiving all dogs.");
                return new List<Dog>();
            }
        }
        public async Task<List<UserDogDTO>> GetAll(int userId)
        {
            try
            {
                List<Dog> dogs = await _dogRepository.GetAllUserDogsDb(userId);
                List<UserDogDTO> ListToReturn = new List<UserDogDTO>();
                foreach (Dog dog in dogs)
                {
                    ListToReturn.Add(new UserDogDTO
                    {
                        Id = dog.Id,
                        Name = dog.Name,
                        ImageUrl = dog.Images.FirstOrDefault(i => i.Order == 0)?.Url
                    });
                }
                _logger.LogInformation("Successfully retrieved {ListToReturn.Count} dogs.", ListToReturn.Count);

                return ListToReturn;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred gettin all user dogs.");
                return new List<UserDogDTO>();
            }
        }

        public async Task<List<DogDTO>> GetUnmatchedDogs(int dogId, DogsFilterDTO filters)
        {
            try
            {
                Dog? originDog = await _dogRepository.GetDogByIdDb(dogId);
                if (originDog == null)
                    return new List<DogDTO>();

                double lat = originDog.Lat;
                double lon = originDog.Lon;

                double? radiusKm = filters.Distance;
                double earthRadiusKm = 6371;

                double deltaLat = radiusKm.HasValue ? radiusKm.Value / earthRadiusKm * (180 / Math.PI) : 90;
                double deltaLon = radiusKm.HasValue ? radiusKm.Value / (earthRadiusKm * Math.Cos(lat * Math.PI / 180)) * (180 / Math.PI) : 180;

                var allUnmatchedDogs = await _dogRepository.GetUnMatchedDogsDb(dogId);

                var boxFilteredDogs = allUnmatchedDogs
                    .Where(d =>
                        d.Lat >= lat - deltaLat && d.Lat <= lat + deltaLat &&
                        d.Lon >= lon - deltaLon && d.Lon <= lon + deltaLon)
                    .ToList();

                var origin = new EntityDistance { EntityId = originDog.Id, Lat = lat, Lon = lon };
                var candidates = boxFilteredDogs
                    .Select(d => new EntityDistance { EntityId = d.Id, Lat = d.Lat, Lon = d.Lon })
                    .ToList();

                var dogsWithDistance = MapDistanceHelper.CalculateDistance(origin, candidates);

                var finalDogsList = dogsWithDistance
                    .Select(d => new
                    {
                        Dog = boxFilteredDogs.First(x => x.Id == d.EntityId),
                        Distance = d.Distance
                    })
                    .Where(x =>
                        (filters.Distance == null || x.Distance <= filters.Distance.Value) &&
                        (filters.Breeds == null || filters.Breeds.Contains(x.Dog.Type)) &&
                        (filters.Size == null || filters.Size.Contains(x.Dog.Size)) &&
                        (filters.Gender == null || filters.Gender.Contains(x.Dog.Gender)) &&
                        (filters.Vaccinated == null || filters.Vaccinated.Contains(x.Dog.Vaccinated)))
                    .Select(x => new DogDTO
                    {
                        Id = x.Dog.Id,
                        Name = x.Dog.Name,
                        Description = x.Dog.Description,
                        Type = x.Dog.Type,
                        Size = x.Dog.Size,
                        Gender = x.Dog.Gender,
                        BirthDate = x.Dog.BirthDate,
                        Vaccinated = x.Dog.Vaccinated,
                        Distance = x.Distance,
                        Images = x.Dog.Images.OrderBy(i => i.Order).Select(i => new ImageDTO
                        {
                            Id = i.Id,
                            Url = i.Url
                        }).ToList()
                    })
                    .ToList();

                _logger.LogInformation("Retrieved {Count} unmatched dogs with real distances.", finalDogsList.Count);
                return finalDogsList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving unmatched dogs.");
                return new List<DogDTO>();
            }
        }

        //public async Task<List<DogDTO>> GetUnmatchedDogs(int dogId, DogsFilterDTO filters)
        //{
        //    try
        //    {
        //        List<Dog> unmatchedDogs = await _dogRepository.GetUnMatchedDogsDb(dogId);
        //        Dog? originDog = await _dogRepository.GetDogByIdDb(dogId);
        //        if (originDog == null)
        //        {
        //            return new List<DogDTO>();
        //        }
        //        EntityDistance originDogLocation = new EntityDistance
        //        {
        //            EntityId = originDog.Id,
        //            Lat = originDog.Lat,
        //            Lon = originDog.Lon,
        //        };

        //        List<EntityDistance> dogsDistance = unmatchedDogs
        //            .Select(dog => new EntityDistance { EntityId = dog.Id, Lat = dog.Lat, Lon = dog.Lon }).ToList();

        //        List<EntityDistance> updatedDogsDistance = MapDistanceHelper.CalculateDistance(originDogLocation, dogsDistance);

        //        List<DogDTO> finalDogsList = new List<DogDTO>();
        //        foreach (EntityDistance dog in updatedDogsDistance)
        //        {
        //            Dog? currentDog = unmatchedDogs.FirstOrDefault(d => d.Id == dog.EntityId);

        //            if (currentDog != null
        //                && (filters.Distance == null || filters.Distance >= dog.Distance)
        //                && (filters.Breeds == null || filters.Breeds.Contains(currentDog.Type))
        //                && (filters.Size == null || filters.Size.Contains(currentDog.Size))
        //                && (filters.Gender == null || filters.Gender.Contains(currentDog.Gender))
        //                && (filters.Vaccinated == null || filters.Vaccinated.Contains(currentDog.Vaccinated))
        //                )
        //            {

        //                List<ImageDTO> dogImages = new List<ImageDTO>();
        //                foreach (Image image in currentDog.Images.OrderBy(d => d.Order))
        //                {
        //                    dogImages.Add(new ImageDTO
        //                    {
        //                        Id = image.Id,
        //                        Url = image.Url
        //                    });
        //                }
        //                finalDogsList.Add(new DogDTO
        //                {
        //                    Id = dog.EntityId,
        //                    Name = currentDog.Name,
        //                    Description = currentDog.Description,
        //                    Type = currentDog.Type,
        //                    Size = currentDog.Size,
        //                    Gender = currentDog.Gender,
        //                    BirthDate = currentDog.BirthDate,
        //                    Distance = dog.Distance,
        //                    Vaccinated = currentDog.Vaccinated,
        //                    Images = dogImages,
        //                });
        //            }
        //        }
        //        _logger.LogInformation("Successfully retrieved {finalDogsList.Count} dogs.", finalDogsList.Count);

        //        return finalDogsList;
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e, "Error occurred receiving unmatched dogs."); 
        //        return new List<DogDTO>();

        //    }
        //}

        public async Task<DogDTO?> GetOne(int id, bool isOwner)
        {
            try
            {
                Dog? dog = await _dogRepository.GetDogByIdDb(id);
                if (dog == null)
                {
                    return null;
                }
                List<ImageDTO> dogImages = new List<ImageDTO>();
                foreach (Image image in dog.Images.OrderBy(d => d.Order))
                {
                    dogImages.Add(new ImageDTO
                    {
                        Id = image.Id,
                        Url = image.Url
                    });
                }
                DogDTO dogToReturn = new DogDTO
                {
                    Id = dog.Id,
                    Name = dog.Name,
                    Description = dog.Description,
                    Type = dog.Type,
                    Size = dog.Size,
                    Gender = dog.Gender,
                    BirthDate = dog.BirthDate,
                    Images = dogImages,
                    Vaccinated = dog.Vaccinated,
                };
                if (isOwner)
                {
                    dogToReturn.Address = dog.Address;
                    dogToReturn.Lon = dog.Lon;
                    dogToReturn.Lat = dog.Lat;
                }
                _logger.LogInformation("Successfully retrieved dog {dog.Id} .", dog.Id);

                return dogToReturn;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred receiving one dog.");
                return null;
            }
        }

        public async Task<Dog?> Update(int id, Dog dog)
        {
            try
            {
                return await _dogRepository.UpdateDogDb(id, dog);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while updating dog."); 
                return null;
            }
        }
        public async Task<DogDTO?> Delete(int id)
        {
            try
            {
                Dog? dogToDelete = await _dogRepository.GetDogByIdDb(id);
                if (dogToDelete == null) return null;

                List<Match> allMatches = dogToDelete.MatchesAsSender.Concat(dogToDelete.MatchesAsReceiver).ToList();
                List<Chat> allChats = dogToDelete.ChatsAsSender.Concat(dogToDelete.ChatsAsReceiver).ToList();

                User? user = await _userRepository.GetUserByIdDb(dogToDelete.UserId);
                if (user == null) return null;

                user.Dogs.Remove(dogToDelete);

                string? refreshToken = _jwtService.GenerateToken(user);
                if (refreshToken == null) return null;

                foreach (Match match in allMatches)
                {
                    await _matchRepository.DeleteMatchDb(match.Id);
                }
                foreach (Chat chat in allChats)
                {
                    await _chatRepository.DeleteChatDb(chat.Id);
                }

                Dog? deletedDog = await _dogRepository.DeleteDogDb(id);
                if (deletedDog == null) return null;

                DogDTO dogToReturn = new DogDTO
                {
                    Id = deletedDog.Id,
                    Name = deletedDog.Name,
                    RefreshToken = refreshToken
                };
                if (dogToReturn == null) return null;

                _logger.LogInformation("Successfully deleted dog {deletedDog.Id} .", deletedDog.Id);

                return dogToReturn;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while deleting dog."); 
                return null;
            }
        }
    }
}
