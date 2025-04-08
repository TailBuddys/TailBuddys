using TailBuddys.Application.Interfaces;
using TailBuddys.Application.Utils;
using TailBuddys.Core.DTO;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.DTO;
using TailBuddys.Core.Models.SubModels;

namespace TailBuddys.Application.Services
{
    // סינון כלבים לפי מרחקים - גט אןמאץ'ד דוגס
    public class DogService : IDogService
    {
        private readonly IDogRepository _dogRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IImageService _imageService;
        private readonly IUserService _userService;
        private readonly IAuth _jwtService;

        public DogService(
            IDogRepository dogRepository,
            IMatchRepository matchRepository,
            IChatRepository chatRepository,
            IImageService imageService,
            IUserService userService,
            IAuth jwtService)
        {
            _dogRepository = dogRepository;
            _matchRepository = matchRepository;
            _chatRepository = chatRepository;
            _imageService = imageService;
            _userService = userService;
            _jwtService = jwtService;
        }

        public async Task<DogDTO?> Create(Dog dog, int userId)
        {
            // לעדכן ליוזר את הכלב הפעיל האחרון
            try
            {
                if (dog == null) return null;
                dog.UserId = userId;
                User? user = await _userService.GetOne(userId);
                if (user == null) return null;

                Dog? dogToCreate = await _dogRepository.CreateDogDb(dog);
                if (dogToCreate == null) return null;


                string? refreshToken = _jwtService.GenerateToken(user);
                if (refreshToken == null) return null;

                DogDTO dogToReturn = new DogDTO
                {
                    Id = dogToCreate.Id,
                    Name = dogToCreate.Name,
                    Description = dogToCreate.Description,
                    Type = dogToCreate.Type,
                    Size = dogToCreate.Size,
                    Gender = dogToCreate.Gender,
                    BirthDate = dogToCreate.BirthDate,
                    Images = dogToCreate.Images.Select(image => image.Url).ToList(),
                    Vaccinated = dogToCreate.Vaccinated,
                    RefreshToken = refreshToken
                };
                if (dogToReturn == null) return null;

                return dogToReturn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                Console.WriteLine(e);
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
                return ListToReturn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<UserDogDTO>();
            }
        }

        public async Task<List<DogDTO>> GetUnmatchedDogs(int dogId, DogsFilterDTO filters)
        {
            try
            {
                List<Dog> unmatchedDogs = await _dogRepository.GetUnMatchedDogsDb(dogId);
                Dog? originDog = await _dogRepository.GetDogByIdDb(dogId);
                EntityDistance originDogLocation = new EntityDistance
                {
                    EntityId = originDog.Id,
                    Lat = originDog.Lat,
                    Lon = originDog.Lon,
                };

                List<EntityDistance> dogsDistance = unmatchedDogs
                    .Select(dog => new EntityDistance { EntityId = dog.Id, Lat = dog.Lat, Lon = dog.Lon }).ToList();

                List<EntityDistance> updatedDogsDistance = MapDistanceHelper.CalculateDistance(originDogLocation, dogsDistance);

                List<DogDTO> finalDogsList = new List<DogDTO>();

                foreach (EntityDistance dog in updatedDogsDistance)
                {
                    Dog? currentDog = unmatchedDogs.FirstOrDefault(d => d.Id == dog.EntityId);

                    if (currentDog != null
                        && (filters.distance == null || filters.distance >= dog.Distance)
                        && (filters.Type == null || filters.Type.Contains(currentDog.Type))
                        && (filters.Size == null || filters.Size.Contains(currentDog.Size))
                        && (filters.Gender == null || filters.Gender == currentDog.Gender)
                        && (filters.Vaccinated == null || filters.Vaccinated == currentDog.Vaccinated))
                    {
                        finalDogsList.Add(new DogDTO
                        {
                            Id = dog.EntityId,
                            Name = currentDog.Name,
                            Description = currentDog.Description,
                            Type = currentDog.Type,
                            Size = currentDog.Size,
                            Gender = currentDog.Gender,
                            BirthDate = currentDog.BirthDate,
                            Distance = dog.Distance,
                            Vaccinated = currentDog.Vaccinated,
                            Images = currentDog.Images.Select(image => image.Url).ToList()
                        });
                    }
                }
                return finalDogsList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<DogDTO>();
            }
        }

        public async Task<DogDTO?> GetOne(int id, bool isOwner)
        {
            try
            {
                Dog? dog = await _dogRepository.GetDogByIdDb(id);
                DogDTO dogToReturn = new DogDTO
                {
                    Id = dog.Id,
                    Name = dog.Name,
                    Description = dog.Description,
                    Type = dog.Type,
                    Size = dog.Size,
                    Gender = dog.Gender,
                    BirthDate = dog.BirthDate,
                    Images = dog.Images.Select(image => image.Url).ToList(),
                    Vaccinated = dog.Vaccinated,
                };
                if (isOwner)
                {
                    dogToReturn.Address = dog.Address;
                    dogToReturn.Lon = dog.Lon;
                    dogToReturn.Lat = dog.Lat;
                }
                return dogToReturn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
                Console.WriteLine(e);
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

                User? user = await _userService.GetOne(dogToDelete.UserId);
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
                foreach (Image image in dogToDelete.Images)
                {
                    await _imageService.RemoveImage(image.Id, dogToDelete.Id, 0);
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

                return dogToReturn;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
