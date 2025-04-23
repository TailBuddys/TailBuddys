using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Models;
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
            if (!Context.User.Claims.Any(c => c.Type == "DogId" && c.Value == dogId.ToString()))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized dog access."); 
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"DogChats_{dogId}");
            _tracker.JoinDogChatsGroup(dogId);

            await SendInitialNotifications(dogId);
        }

        public async Task SendInitialNotifications(int dogId)
        {
            List<ChatNotification> notifications = await _notificationService.GetAllDogChatsNotifications(dogId);

            foreach (ChatNotification notification in notifications)
            {
                await Clients.Caller.SendAsync("ReceiveChatNotification", notification);
            }
        }

        public async Task LeaveDogChatsGroup(int dogId)
        {
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

            if (!Context.User.Claims.Any(c => c.Type == "DogId" && c.Value == dogId.ToString()))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized dog access.");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
            var chatNotification = await _notificationService.GetChatNotificationsById(chatId, dogId);
            if (chatNotification != null)
            {
                await Clients.Caller.SendAsync("ReceiveChatNotification", chatNotification);
                await _notificationService.DeleteChatNotifications(chatId, dogId);
            }
            var allDogsInChat = _tracker.GetAllDogsInChat(chatId);
            foreach (var otherDogId in allDogsInChat)
            {
                if (otherDogId == dogId) continue;

                if (_tracker.IsDogInSpecificChat(otherDogId, chatId))
                {
                    await Clients.Group($"Chat_{chatId}").SendAsync("ReceiveChatNotification", new
                    {
                        chatId,
                        joinedDogId = dogId,
                        message = $"Dog {dogId} joined the chat"
                    });
                }
            }
        }

        public async Task LeaveSpecificChatGroup(int chatId, int dogId)
        {
            _tracker.LeaveChat(dogId, chatId);
            if (!Context.User.Claims.Any(c => c.Type == "DogId" && c.Value == dogId.ToString()))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized dog access.");
                return;
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var dogIdClaim = Context.User.Claims.Where(c => c.Type == "DogId");
            foreach (var dog in dogIdClaim)
            {
                if (dogIdClaim != null && int.TryParse(dog.Value, out int dogId))
                {
                    _tracker.LeaveDogChatsGroup(dogId);

                    var chats = _tracker.GetAllDogsInChat(dogId);
                    foreach (var chatId in chats)
                    {
                        _tracker.LeaveChat(dogId, chatId);
                        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
                    }
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}
