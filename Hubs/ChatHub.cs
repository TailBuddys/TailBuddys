using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TailBuddys.Application.Interfaces;
using TailBuddys.Hubs.HubInterfaces;

namespace TailBuddys.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly INotificationService _notificationService;
        private readonly IDogConnectionTracker _tracker;


        public ChatHub(INotificationService notificationService, IDogConnectionTracker tracker)
        {
            _notificationService = notificationService;
            _tracker = tracker;
        }

        public async Task JoinDogChatsGroup(int dogId)
        {
            // ✅ Check if connected user owns the dog they're trying to subscribe to
            if (!Context.User.Claims.Any(c => c.Type == "DogId" && c.Value == dogId.ToString()))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized dog access."); // ✅ Prevent unauthorized group access
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"DogChats_{dogId}");
            _tracker.JoinDogChatsGroup(dogId);

            var notifications = await _notificationService.GetAllDogChatsNotifications(dogId);
            foreach (var notification in notifications)
            {
                await Clients.Caller.SendAsync("ReceiveChatNotification", new { chatId = notification });
            }
        }

        public async Task LeaveDogChatsGroup(int dogId)
        {
            // ✅ Still check ownership for completeness
            if (!Context.User.Claims.Any(c => c.Type == "DogId" && c.Value == dogId.ToString()))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized dog access.");
                return;
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"DogChats_{dogId}");
            _tracker.LeaveDogChatsGroup(dogId);
        }

        public async Task JoinSpecificChatGroup(int chatId, int dogId)
        {
            _tracker.JoinChat(dogId, chatId);

            // ✅ Security: validate dog ownership before joining chat group
            if (!Context.User.Claims.Any(c => c.Type == "DogId" && c.Value == dogId.ToString()))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized dog access.");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
            var chatNotification = await _notificationService.GetChatNotificationsById(chatId, dogId);
            if (chatNotification != null)
            {
                await Clients.Caller.SendAsync("ReceiveChatNotification", new { chatId });
                await _notificationService.DeleteChatNotifications(chatId, dogId);
            }
        }

        public async Task LeaveSpecificChatGroup(int chatId, int dogId)
        {
            _tracker.LeaveChat(dogId, chatId);

            // ✅ Same security enforcement as join
            if (!Context.User.Claims.Any(c => c.Type == "DogId" && c.Value == dogId.ToString()))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized dog access.");
                return;
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var dogIdClaim = Context.User.Claims.FirstOrDefault(c => c.Type == "DogId");
            if (dogIdClaim != null && int.TryParse(dogIdClaim.Value, out int dogId))
            {
                _tracker.LeaveDogChatsGroup(dogId);

                var chats = _tracker.GetAllChatsForDog(dogId);
                foreach (var chatId in chats)
                {
                    _tracker.LeaveChat(dogId, chatId);
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}
