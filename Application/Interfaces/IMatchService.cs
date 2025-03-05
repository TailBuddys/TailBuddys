using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IMatchService
    {
        Task<Match?> CreateMatch(Match match);
        Task<List<Match>> GetAllMutualMatches(int dogId); // לשימוש של הפרונט
        Task<List<Match>> GetAllMatchesAsSenderDog(int dogId); // לשימוש פנימי של הסרוויס ??????
        Task<List<Match>> GetAllMatchesAsReciverDog(int dogId); // לשימוש פנימי של הסרוויס ?????
        Task<Match?> GetMatchById(int matchId);
        Task<Match?> UpdateMatch(int matchId, Match newMatch);
        Task<Match?> DeleteMatch(int matchId);
    }
}
