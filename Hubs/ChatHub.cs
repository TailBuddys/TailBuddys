using Microsoft.AspNetCore.SignalR;
using TailBuddys.Application.Interfaces;

namespace TailBuddys.Hubs
{
    public class ChatHub : Hub
    {
        private readonly INotificationService _notificationService;

        public ChatHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task JoinDogChatsGroup(int dogId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"DogChats_{dogId}");
        }

        public async Task LeaveDogChatsGroup(int dogId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"DogChats_{dogId}");
        }

        public async Task JoinSpecificChatGroup(int chatId, int dogId)
        {
            ChatHubTracker.JoinChat(dogId, chatId);
            await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        }

        public async Task LeaveSpecificChatGroup(int chatId, int dogId)
        {
            ChatHubTracker.LeaveChat(dogId, chatId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        }
    }
}
