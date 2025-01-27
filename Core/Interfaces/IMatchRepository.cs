using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IMatchRepository
    {
        // dog will create a new match for like / unlike new dog - need to check at this point if other dog like him too 
        public Task<Match?> CreateMatch(string id);
        // get all dog matches that have IsMatch = true
        public Task<List<Match?>> GetAllMatches(string id);
        // get all dog matches that he like / unlike 
        public Task<List<Match?>> GetAllMatchesFromDog(string id);
        // get all dog matches from other dogs
        public Task<List<Match?>> GetAllMatchesToDog(string id);
        public Task<Match?> GetMatchById(string id);
        public Task<Match?> UpdateMatch(string id);
        public Task<Match?> DeleteMatch(string id);
    }
}
