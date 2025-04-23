using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IMatchRepository
    {
        public Task<Match?> CreateMatchDb(Match match);
        public Task<List<Match>> GetAllMutualMatchesDb(int dogId);
        public Task<List<Match>> GetAllMatchesAsSenderDogDb(int dogId);
        public Task<List<Match>> GetAllMatchesAsReceiverDogDb(int dogId);
        public Task<Match?> GetMatchByIdDb(int matchId);
        public Task<Match?> UpdateMatchDb(int matchId, Match match);
        public Task<Match?> DeleteMatchDb(int matchId);
    }
}
