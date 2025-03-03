using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IMatchService
    {
        Task<Match?> CreateMatch(Match match);
        Task<List<Match>> GetAllMutualMatches(string dogId); // לשימוש של הפרונט
        Task<List<Match>> GetAllMatchesAsSenderDog(string dogId); // לשימוש פנימי של הסרוויס ??????
        Task<List<Match>> GetAllMatchesAsReciverDog(string dogId); // לשימוש פנימי של הסרוויס ?????
        Task<Match?> GetMatchById(int matchId);
        Task<Match?> UpdateMatch(int matchId, Match newMatch);
        Task<Match?> DeleteMatch(int matchId);
    }
}
