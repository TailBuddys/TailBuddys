using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }
        public async Task<Match?> CreateMatch(Match match)
        {
            // לדאוג שאם מאץ' קיים לכלב הממאצ'מץ' לא ליצור עוד אחד אלא לעדכן את הקיים
            try
            {
                if (match == null) return null;
                match.CreatedAt = DateTime.Now;
                match.UpdatedAt = DateTime.Now;

                Match? foreignMatch = _matchRepository.GetAllMatchesFromDog(match.ToDogId)
                    .Result.Where(m => m.ToDogId == match.FromDogId).FirstOrDefault();

                if (foreignMatch == null)
                {
                    return await _matchRepository.CreateMatch(match);
                }

                if (match.IsLike && foreignMatch.IsLike)
                {
                    match.IsMatch = true;
                    foreignMatch.IsMatch = true;
                    foreignMatch.UpdatedAt = DateTime.Now;
                    await _matchRepository.UpdateMatch(foreignMatch.Id, foreignMatch);
                    return await _matchRepository.CreateMatch(match);
                }
                return await _matchRepository.CreateMatch(match);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Match>> GetAllMutualMatches(string dogId)
        {
            // ליצור מודל של כלב עם תמונה ושם להחזרה לפרונט
            // לבנות מודל DTO
            try
            {
                return await _matchRepository.GetAllMutualMatches(dogId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Match>> GetAllMatchesFromDog(string dogId)
        {
            try
            {
                return await _matchRepository.GetAllMatchesFromDog(dogId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Match>> GetAllMatchesToDog(string dogId)
        {
            try
            {
                return await _matchRepository.GetAllMatchesToDog(dogId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<Match?> GetMatchById(int matchId)
        {
            try
            {
                return await _matchRepository.GetMatchById(matchId);
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
                return await _matchRepository.UpdateMatch(matchId, newMatch);
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
                return await _matchRepository.DeleteMatch(matchId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
