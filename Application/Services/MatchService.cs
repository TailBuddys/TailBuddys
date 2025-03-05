using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IDogRepository _dogRepository;

        public MatchService(IMatchRepository matchRepository, IDogRepository dogRepository)
        {
            _matchRepository = matchRepository;
            _dogRepository = dogRepository;
        }
        public async Task<Match?> CreateMatch(Match match)
        {
            // צריך להכליל בפונקציה הזו גם עדכון של נוטיפיקיישן סרוויס
            try
            {

                if (match.ReciverDogId == 0 || match.SenderDogId == 0) return null;

                (Dog? reciverDog, Dog? senderDog) = (await _dogRepository.GetDogByIdDb(match.ReciverDogId),
                    await _dogRepository.GetDogByIdDb(match.SenderDogId));

                if (senderDog?.UserId == reciverDog?.UserId ||
                    (await _matchRepository.GetAllMatchesAsSenderDogDb(match.SenderDogId))
                    .Any(m => m.ReciverDogId == match.ReciverDogId)) return null;

                match.CreatedAt = DateTime.Now;
                match.UpdatedAt = DateTime.Now;

                Match? foreignMatch = _matchRepository.GetAllMatchesAsSenderDogDb(match.ReciverDogId)
                    .Result.Where(m => m.ReciverDogId == match.SenderDogId).FirstOrDefault();

                if (foreignMatch == null)
                {
                    return await _matchRepository.CreateMatchDb(match);
                }

                if (match.IsLike && foreignMatch.IsLike)
                {
                    match.IsMatch = true;
                    foreignMatch.IsMatch = true;
                    foreignMatch.UpdatedAt = DateTime.Now;
                    await _matchRepository.UpdateMatchDb(foreignMatch.Id, foreignMatch);
                    return await _matchRepository.CreateMatchDb(match);
                }
                return await _matchRepository.CreateMatchDb(match);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Match>> GetAllMutualMatches(int dogId)
        {
            // ליצור מודל של כלב עם תמונה ושם להחזרה לפרונט
            // לבנות מודל DTO
            try
            {
                return await _matchRepository.GetAllMutualMatchesDb(dogId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Match>();
            }
        }
        public async Task<List<Match>> GetAllMatchesAsSenderDog(int dogId)
        {
            try
            {
                return await _matchRepository.GetAllMatchesAsSenderDogDb(dogId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Match>();
            }
        }
        public async Task<List<Match>> GetAllMatchesAsReciverDog(int dogId)
        {
            try
            {
                return await _matchRepository.GetAllMatchesAsReciverDogDb(dogId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Match>();
            }
        }
        public async Task<Match?> GetMatchById(int matchId)
        {
            try
            {
                return await _matchRepository.GetMatchByIdDb(matchId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Match?> UpdateMatch(int matchId, Match newMatch)
        {
            try
            {
                Match? matchToUpdate = await _matchRepository.GetMatchByIdDb(matchId);
                if (matchToUpdate == null) return null;

                if (newMatch.IsLike == false && matchToUpdate.IsMatch == true)
                {
                    Match? foreignMatch = _matchRepository.GetAllMatchesAsSenderDogDb(newMatch.ReciverDogId)
                    .Result.Where(m => m.ReciverDogId == newMatch.SenderDogId).FirstOrDefault();

                    if (foreignMatch != null)
                    {
                        foreignMatch.IsMatch = false;
                        await _matchRepository.UpdateMatchDb(foreignMatch.Id, foreignMatch);
                    }
                    newMatch.IsMatch = false;
                }
                return await _matchRepository.UpdateMatchDb(matchId, newMatch);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<Match?> DeleteMatch(int matchId)
        {
            try
            {
                return await _matchRepository.DeleteMatchDb(matchId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
