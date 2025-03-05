using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IMatchRepository
    {
        public Task<Match?> CreateMatchDb(Match match);
        // get all dog matches that have IsMatch = true
        public Task<List<Match>> GetAllMutualMatchesDb(int dogId);
        // get all dog matches that he like / unlike 
        public Task<List<Match>> GetAllMatchesAsSenderDogDb(int dogId);
        // get all dog matches from other dogs
        public Task<List<Match>> GetAllMatchesAsReciverDogDb(int dogId);
        public Task<Match?> GetMatchByIdDb(int matchId);
        public Task<Match?> UpdateMatchDb(int matchId, Match match);
        public Task<Match?> DeleteMatchDb(int matchId);
    }
}
