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
            // צריך להכליל בפונקציה הזו גם עדכון של נוטיפיקיישן סרוויס
            // לדאוג שאם מאץ' קיים לכלב השולח לא ליצור עוד אחד אלא לעדכן את הקיים??
            try
            {

                if (match == null || match.SenderDog.UserId == match.ReciverDog.UserId) return null;
                // בודק שאני לא עושה לעצמי -- לא מביא את הכלב -- לא תקין
                match.CreatedAt = DateTime.Now;
                match.UpdatedAt = DateTime.Now;

                if ((await _matchRepository.GetAllMatchesAsSenderDogDb(match.SenderDogId)) // בודק שאני לא עושה מאץ' כפול
                    .Any(m => m.SenderDogId == match.ReciverDogId))
                    return null;

                Match? foreignMatch = _matchRepository.GetAllMatchesAsSenderDogDb(match.ReciverDogId)
                    .Result.Where(m => m.ReciverDogId == match.SenderDogId).FirstOrDefault();

                if (foreignMatch == null) // בודק אם קיים בצד של המקבל
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
                return await _matchRepository.CreateMatchDb(match); // במידה וקיים בצד המקבל אבל הוא לא אהב אותי
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
                return await _matchRepository.GetAllMutualMatchesDb(dogId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Match>();
            }
        }
        public async Task<List<Match>> GetAllMatchesFromDog(string dogId)
        {
            try
            {
                return await _matchRepository.GetAllMatchesAsSenderDogDb(dogId);
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
                return await _matchRepository.GetAllMatchesAsReciverDogDb(dogId);
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
                return await _matchRepository.GetMatchByIdDb(matchId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        // לדאוג שאןמאץ' מבטל את המאץ' גם מהצד של הכלב המקבל
        public async Task<Match?> UpdateMatch(int matchId, Match newMatch)
        {
            try
            {
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
