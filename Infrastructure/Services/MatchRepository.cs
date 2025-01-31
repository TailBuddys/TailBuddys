using Microsoft.EntityFrameworkCore;
using TailBuddys.Core.Models;
using TailBuddys.Infrastructure.Data;

namespace TailBuddys.Infrastructure.Services
{
    public class MatchRepository
    {
        private readonly TailBuddysContext _context;
        public MatchRepository(TailBuddysContext context)
        {
            _context = context;
        }
        public async Task<Match?> CreateMatch(Match match)
        {
            try
            {
                _context.Matches.Add(match);
                await _context.SaveChangesAsync();
                return match;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<List<Match>> GetAllMutualMatches(string dogId)
        {
            try
            {
                List<Match> list = await _context.Matches.Where(m=>m.FromDogId == dogId && m.IsMatch == true).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Match>();
            }
        }
        // get async all dog matches that he like / unlike 
        public async Task<List<Match>> GetAllMatchesFromDog(string dogId)
        {
            try
            {
                List<Match> list = await _context.Matches.Where(m => m.FromDogId == dogId).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Match>();
            }
        }
        // get all dog that like me
        public async Task<List<Match>> GetAllMatchesToDog(string dogId)
        {
            try
            {
                List<Match> list = await _context.Matches.Where(m => m.ToDogId == dogId && m.IsLike == true).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<Match>();
            }
        }
        public async Task<Match?> GetMatchById(int matchId)
        {
            try
            {
                Match? match = await _context.Matches.FirstOrDefaultAsync(m => m.Id == matchId);
                return match;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<Match?> UpdateMatch(int matchId, Match newMatch)
        {
            try
            {
                Match? matchToUpdate = await _context.Matches.FirstOrDefaultAsync(m => m.Id == matchId);
                if (matchToUpdate == null)
                {
                    return null;
                }
                
                matchToUpdate.IsMatch = newMatch.IsMatch;
                matchToUpdate.IsLike = newMatch.IsLike;
                matchToUpdate.UpdatedAt = DateTime.Now;
                
                _context.Matches.Update(matchToUpdate);
                await _context.SaveChangesAsync();
                return matchToUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<Match?> DeleteMatch(int matchId)
        {
            try
            {
                Match? matchToRemove = await _context.Matches.FirstOrDefaultAsync(m => m.Id == matchId);
                if (matchToRemove == null)
                {
                    return null;
                }
                _context.Matches.Remove(matchToRemove);
                await _context.SaveChangesAsync();
                return matchToRemove;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
    }
}
