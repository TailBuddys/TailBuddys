using Microsoft.EntityFrameworkCore;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Infrastructure.Data;

namespace TailBuddys.Infrastructure.Services
{
    public class MatchRepository : IMatchRepository
    {
        private readonly TailBuddysContext _context;
        private readonly ILogger<MatchRepository> _logger;

        public MatchRepository(TailBuddysContext context, ILogger<MatchRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Match?> CreateMatchDb(Match match)
        {
            try
            {
                match.CreatedAt = DateTime.Now;
                match.UpdatedAt = DateTime.Now;
                _context.Matches.Add(match);
                await _context.SaveChangesAsync();
                return match;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating new match.");
                return null;
            }
        }
        public async Task<List<Match>> GetAllMutualMatchesDb(int dogId)
        {
            try
            {
                List<Match> list = await _context.Matches
                     .Include(m => m.SenderDog)
                     .Include(m => m.ReceiverDog!)
                     .ThenInclude(d => d.Images)
                     .Where(m => m.SenderDogId == dogId && m.IsMatch == true)
                     .ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all mutual matchs.");
                return new List<Match>();
            }
        }
        public async Task<List<Match>> GetAllMatchesAsSenderDogDb(int dogId)
        {
            try
            {
                List<Match> list = await _context.Matches
                    .Include(m => m.SenderDog)
                    .Include(m => m.ReceiverDog)
                    .Where(m => m.SenderDogId == dogId).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all mutual matches as sender.");
                return new List<Match>();
            }
        }
        public async Task<List<Match>> GetAllMatchesAsReceiverDogDb(int dogId)
        {
            try
            {
                List<Match> list = await _context.Matches
                    .Include(m => m.SenderDog)
                    .Include(m => m.ReceiverDog)
                    .Where(m => m.ReceiverDogId == dogId).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all mutual matches as receiver.");
                return new List<Match>();
            }
        }
        public async Task<Match?> GetMatchByIdDb(int matchId)
        {
            try
            {
                Match? match = await _context.Matches
                    .Include(m => m.SenderDog)
                    .Include(m => m.ReceiverDog)
                    .FirstOrDefaultAsync(m => m.Id == matchId);
                return match;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting match by id.");
                return null;
            }
        }
        public async Task<Match?> UpdateMatchDb(int matchId, Match newMatch)
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
                _logger.LogError(ex, "Error occurred while updating match.");
                return null;
            }
        }
        public async Task<Match?> DeleteMatchDb(int matchId)
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
                _logger.LogError(ex, "Error occurred while deleting match.");
                return null;
            }
        }
    }
}
