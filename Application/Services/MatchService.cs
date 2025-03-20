using Microsoft.AspNetCore.SignalR;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Hubs;

namespace TailBuddys.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IDogRepository _dogRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService _notificationService;
        private readonly HashSet<int> _activeDogs;

        public MatchService(
            IMatchRepository matchRepository,
            IDogRepository dogRepository,
            IHubContext<NotificationHub> hubContext,
            INotificationService notificationService,
            HashSet<int> activeDogs
            )
        {
            _matchRepository = matchRepository;
            _dogRepository = dogRepository;
            _hubContext = hubContext;
            _notificationService = notificationService;
            _activeDogs = activeDogs;
        }
        public async Task<Match?> CreateMatch(Match match)
        {
            // צריך להכליל בפונקציה הזו גם עדכון של נוטיפיקיישן סרוויס
            try
            {

                if (match.ReciverDogId == 0 || match.SenderDogId == 0) return null;

                (Dog? reciverDog, Dog? senderDog) = (await _dogRepository.GetDogByIdDb(match.ReciverDogId),
                    await _dogRepository.GetDogByIdDb(match.SenderDogId));

                if (senderDog?.UserId == reciverDog?.UserId ||
                    (await _matchRepository.GetAllMatchesAsSenderDogDb(match.SenderDogId))
                    .Any(m => m.ReciverDogId == match.ReciverDogId)) return null;

                match.CreatedAt = DateTime.Now;
                match.UpdatedAt = DateTime.Now;

                Match? foreignMatch = _matchRepository.GetAllMatchesAsSenderDogDb(match.ReciverDogId)
                    .Result.Where(m => m.ReciverDogId == match.SenderDogId).FirstOrDefault();

                if (foreignMatch == null)
                {
                    return await _matchRepository.CreateMatchDb(match);
                }

                if (match.IsLike && foreignMatch.IsLike)
                {
                    match.IsMatch = true;
                    foreignMatch.IsMatch = true;
                    foreignMatch.UpdatedAt = DateTime.Now;
                    await _matchRepository.UpdateMatchDb(foreignMatch.Id, foreignMatch);
                    Match? newMatch = await _matchRepository.CreateMatchDb(match);
                    await HandleNewMatch(
                        foreignMatch.SenderDogId,
                        foreignMatch.ReciverDogId,
                        foreignMatch.Id
                        );
                    return newMatch;
                }
                return await _matchRepository.CreateMatchDb(match);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task HandleNewMatch(int senderDogId, int receiverDogId, int matchId)
        {
            bool isActive;
            lock (_activeDogs)
            {
                isActive = _activeDogs.Contains(receiverDogId);
            }

            if (isActive)

            {
                Console.WriteLine("dog is active");
                await _hubContext.Clients.Group(receiverDogId.ToString()).SendAsync("ReceiveNewMatch", matchId);
            }
            else
            {
                Console.WriteLine("dog is not  active");
                await _notificationService.CreateMatchNotification(receiverDogId, matchId);
            }
        }

        public async Task<List<Match>> GetAllMutualMatches(int dogId)
        {
            // ליצור מודל של כלב עם תמונה ושם להחזרה לפרונט
            // לבנות מודל DTO
            try
            {
                return await _matchRepository.GetAllMutualMatchesDb(dogId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Match>();
            }
        }
        public async Task<List<Match>> GetAllMatchesAsSenderDog(int dogId)
        {
            try
            {
                return await _matchRepository.GetAllMatchesAsSenderDogDb(dogId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Match>();
            }
        }
        public async Task<List<Match>> GetAllMatchesAsReciverDog(int dogId)
        {
            try
            {
                return await _matchRepository.GetAllMatchesAsReciverDogDb(dogId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Match>();
            }
        }
        public async Task<Match?> GetMatchById(int matchId)
        {
            try
            {
                return await _matchRepository.GetMatchByIdDb(matchId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task<Match?> UpdateMatch(int matchId, Match newMatch)
        {
            try
            {
                Match? matchToUpdate = await _matchRepository.GetMatchByIdDb(matchId);
                if (matchToUpdate == null) return null;

                if (newMatch.IsLike == false && matchToUpdate.IsMatch == true)
                {
                    Match? foreignMatch = _matchRepository.GetAllMatchesAsSenderDogDb(newMatch.ReciverDogId)
                    .Result.Where(m => m.ReciverDogId == newMatch.SenderDogId).FirstOrDefault();

                    if (foreignMatch != null)
                    {
                        foreignMatch.IsMatch = false;
                        await _matchRepository.UpdateMatchDb(foreignMatch.Id, foreignMatch);
                    }
                    newMatch.IsMatch = false;
                }
                return await _matchRepository.UpdateMatchDb(matchId, newMatch);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<Match?> DeleteMatch(int matchId)
        {
            try
            {
                return await _matchRepository.DeleteMatchDb(matchId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
