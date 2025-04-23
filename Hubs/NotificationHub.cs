using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Hubs.HubInterfaces;

namespace TailBuddys.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly IDogConnectionTracker _tracker;
        private readonly IDogRepository _dogRepository;
        private readonly INotificationService _notificationService;
        private readonly IAuth _jwtAuthService;

        public NotificationHub(
            IDogRepository dogRepository,
            INotificationService notificationService,
            IDogConnectionTracker tracker,
            IAuth jwtAuthService)
        {
            _dogRepository = dogRepository;
            _notificationService = notificationService;
            _tracker = tracker;
            _jwtAuthService = jwtAuthService;
        }

        public async Task<bool> JoinDogGroup(int dogId)
        {
 
            int userId = int.Parse(Context.User?.FindFirst("id")?.Value ?? "0"); 
            if (userId == 0)
                return false;

            var dogs = await _dogRepository.GetAllUserDogsDb(userId);
            if (!dogs.Any(d => d.Id == dogId))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized dog access.");
                return false;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, dogId.ToString());
            _tracker.JoinDogMatchGroup(dogId);

            var matchNotifications = await _notificationService.GetDogAllMatchesNotifications(dogId);
            foreach (var matchNotification in matchNotifications)
            {
                await Clients.Caller.SendAsync("ReceiveNewMatch", matchNotification.MatchId);
            }

            await _notificationService.DeleteMatchesNotifications(dogId);

            return true;
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            int userId = int.Parse(Context.User?.FindFirst("id")?.Value ?? "0");

            var dogs = await _dogRepository.GetAllUserDogsDb(userId);
            foreach (var dog in dogs)
            {
                _tracker.LeaveDogMatchGroup(dog.Id);
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}
