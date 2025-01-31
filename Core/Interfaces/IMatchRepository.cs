using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IMatchRepository
    {
        public Task<Match?> CreateMatch(Match match);
        // get all dog matches that have IsMatch = true
        public Task<List<Match>> GetAllMutualMatches(string dogId);
        // get all dog matches that he like / unlike 
        public Task<List<Match>> GetAllMatchesFromDog(string dogId);
        // get all dog matches from other dogs
        public Task<List<Match>> GetAllMatchesToDog(string dogId);
        public Task<Match?> GetMatchById(int matchId);
        public Task<Match?> UpdateMatch(int matchId, Match match);
        public Task<Match?> DeleteMatch(int matchId);
    }
}
