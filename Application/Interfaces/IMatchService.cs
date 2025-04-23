using TailBuddys.Core.DTO;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IMatchService
    {
        Task<Match?> CreateMatch(Match match);
        Task HandleNewMatch(int senderDogId, int receiverDogId, int matchId);
        Task<List<MatchDTO>> GetAllMutualMatches(int dogId); 
        Task<List<Match>> GetAllMatchesAsSenderDog(int dogId); 
        Task<List<Match>> GetAllMatchesAsReceiverDog(int dogId); 
        Task<Match?> GetMatchById(int matchId);
        Task<Match?> UpdateMatch(int matchId, Match newMatch);
        Task<Match?> DeleteMatch(int matchId);
    }
}
