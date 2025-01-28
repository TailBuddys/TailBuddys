using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IMatchRepository
    {
        // dog will create a new match for like / unlike new dog - need to check at this point if other dog like him too 
        public Task<Match?> CreateMatch(string fromDogId, string toDogId);
        // get all dog matches that have IsMatch = true
        public Task<List<Match?>> GetAllMutualMatches(string dogId);
        // get all dog matches that he like / unlike 
        public Task<List<Match?>> GetAllMatchesFromDog(string dogId);
        // get all dog matches from other dogs
        public Task<List<Match?>> GetAllMatchesToDog(string dogId);
        public Task<Match?> GetMatchById(string matchId);
        public Task<Match?> UpdateMatch(string matchId, Match match);
        public Task<Match?> DeleteMatch(string matchId);
    }
}
