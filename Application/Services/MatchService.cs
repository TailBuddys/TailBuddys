using Microsoft.AspNetCore.SignalR;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.DTO;
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
                if (match.ReceiverDogId == 0 || match.SenderDogId == 0) return null;

                (Dog? receiverDog, Dog? senderDog) = (await _dogRepository.GetDogByIdDb(match.ReceiverDogId),
                    await _dogRepository.GetDogByIdDb(match.SenderDogId));

                if (senderDog?.UserId == receiverDog?.UserId ||
                    (await _matchRepository.GetAllMatchesAsSenderDogDb(match.SenderDogId))
                    .Any(m => m.ReceiverDogId == match.ReceiverDogId)) return null;

                match.CreatedAt = DateTime.Now;
                match.UpdatedAt = DateTime.Now;

                Match? foreignMatch = _matchRepository.GetAllMatchesAsSenderDogDb(match.ReceiverDogId)
                    .Result.Where(m => m.ReceiverDogId == match.SenderDogId).FirstOrDefault();

                if (foreignMatch == null)
                {
                    return await _matchRepository.CreateMatchDb(match);
                }

                else if (senderDog != null && receiverDog != null && receiverDog.IsBot == true)
                {
                    foreignMatch = await _matchRepository.CreateMatchDb(new Match
                    {
                        SenderDogId = receiverDog.Id,
                        ReceiverDogId = senderDog.Id,
                        IsLike = true,
                    });
                }
                if (match.IsLike && foreignMatch != null && foreignMatch.IsLike)

                {
                    match.IsMatch = true;
                    foreignMatch.IsMatch = true;
                    foreignMatch.UpdatedAt = DateTime.Now;
                    await _matchRepository.UpdateMatchDb(foreignMatch.Id, foreignMatch);
                    Match? newMatch = await _matchRepository.CreateMatchDb(match);

                    if (newMatch != null)
                    {
                        if (receiverDog?.IsBot != true)
                        {
                            await HandleNewMatch(
                                foreignMatch.SenderDogId,
                                foreignMatch.ReceiverDogId,
                                foreignMatch.Id
                                );
                        }
                        await HandleNewMatch(
                            newMatch.SenderDogId,
                            newMatch.ReceiverDogId,
                            newMatch.Id
                            );
                    }
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
                isActive = _activeDogs.Contains(senderDogId);
            }

            if (isActive)

            {
                await _hubContext.Clients.Group(senderDogId.ToString()).SendAsync("ReceiveNewMatch", matchId);
            }
            else
            {
                await _notificationService.CreateMatchNotification(senderDogId, matchId);
            }
        }

        public async Task<List<MatchDTO>> GetAllMutualMatches(int dogId)
        {
            // ליצור מודל של כלב עם תמונה ושם להחזרה לפרונט
            // לבנות מודל DTO
            try
            {
                List<Match> matches = await _matchRepository.GetAllMutualMatchesDb(dogId);
                Dog? senderDog = await _dogRepository.GetDogByIdDb(dogId);
                if (senderDog == null) return new List<MatchDTO>();

                List<Match> matchesWithoutChats = matches
                    .Where(m => m.ReceiverDogId != senderDog.ChatsAsReceiver
                    .FirstOrDefault(c => c.SenderDogId == m.ReceiverDogId)?.SenderDogId &&
                     m.ReceiverDogId != senderDog.ChatsAsSender
                     .FirstOrDefault(c => c.ReceiverDogId == m.ReceiverDogId)?.ReceiverDogId).ToList();

                List<MatchDTO> matchDTOs = matchesWithoutChats.Select(m => new MatchDTO
                {
                    Id = m.Id,
                    ReceiverDogId = m.ReceiverDog?.Id ?? 0,
                    ReceiverDogName = m.ReceiverDog?.Name ?? string.Empty,
                    ReceiverDogImage = m.ReceiverDog?.Images?
                    .FirstOrDefault(i => i.Order == 0)?.Url ?? null,
                }).ToList();

                return matchDTOs;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<MatchDTO>();
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
        public async Task<List<Match>> GetAllMatchesAsReceiverDog(int dogId)
        {
            try
            {
                return await _matchRepository.GetAllMatchesAsReceiverDogDb(dogId);
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
                    Match? foreignMatch = _matchRepository.GetAllMatchesAsSenderDogDb(newMatch.ReceiverDogId)
                    .Result.Where(m => m.ReceiverDogId == newMatch.SenderDogId).FirstOrDefault();

                    if (foreignMatch != null)
                    {
                        foreignMatch.IsMatch = false;
                        await _matchRepository.UpdateMatchDb(foreignMatch.Id, foreignMatch);

                        await HandleNewMatch(
                                foreignMatch.SenderDogId,
                                foreignMatch.ReceiverDogId,
                                foreignMatch.Id
                                );
                        await HandleNewMatch(
                                matchToUpdate.SenderDogId,
                                matchToUpdate.ReceiverDogId,
                                matchToUpdate.Id
                                );
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
        public async Task<Match?> DeleteMatch(int matchId) // לא באמת משתמשים בכלל
        {
            try
            {
                Match? match = await _matchRepository.DeleteMatchDb(matchId);
                if (match == null) return null;
                bool isActive;
                lock (_activeDogs)
                {
                    isActive = _activeDogs.Contains(match.ReceiverDogId);
                }

                if (isActive)

                {
                    await _hubContext.Clients.Group(match.ReceiverDogId.ToString()).SendAsync("ReceiveNewMatch", matchId);
                }
                return match;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
