using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Services
{
    // סינון כלבים לפי מרחקים - גט אןמאץ'ד דוגס
    public class DogService : IDogService
    {
        private readonly IDogRepository _dogRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IChatRepository _chatRepository;

        public DogService(IDogRepository dogRepository, IMatchRepository matchRepository, IChatRepository chatRepository)
        {
            _dogRepository = dogRepository;
            _matchRepository = matchRepository;
            _chatRepository = chatRepository;
        }

        public async Task<Dog?> Create(Dog dog, int userId)
        {
            // לעדכן ליוזר את הכלב הפעיל האחרון
            try
            {
                if (dog == null) return null;
                dog.UserId = userId;
                return await _dogRepository.CreateDogDb(dog);
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
        public async Task<List<Dog>> GetAll(int userId)
        {
            try
            {
                return await _dogRepository.GetAllUserDogsDb(userId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Dog>();
            }
        }
        public async Task<List<Dog>> GetUnmatchedDogs(int dogId)
        {
            try
            {

                return await _dogRepository.GetUnMatchedDogsDb(dogId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Dog>();
            }
        }

        public async Task<Dog?> GetOne(int id)
        {
            try
            {
                return await _dogRepository.GetDogByIdDb(id);
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
        public async Task<Dog?> Delete(int id)
        {
            try
            {
                Dog? dogToDelete = await _dogRepository.GetDogByIdDb(id);
                if (dogToDelete == null) return null;

                List<Match> allMatches = dogToDelete.MatchesAsSender.Concat(dogToDelete.MatchesAsReciver).ToList();
                List<Chat> allChats = dogToDelete.ChatsAsSender.Concat(dogToDelete.ChatsAsReciver).ToList();

                foreach (Match match in allMatches)
                {
                    await _matchRepository.DeleteMatchDb(match.Id);
                }
                foreach (Chat chat in allChats)
                {
                    await _chatRepository.DeleteChatDb(chat.Id);
                }

                return await _dogRepository.DeleteDogDb(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
