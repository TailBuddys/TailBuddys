using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using TailBuddys.Application.Interfaces;

namespace TailBuddys.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly INotificationService _notificationService;

        public ChatHub(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        //public async Task JoinDogChatsGroup(int dogId)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, $"DogChats_{dogId}");
        //}

        //public async Task LeaveDogChatsGroup(int dogId)
        //{
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"DogChats_{dogId}");
        //}

        //public async Task JoinSpecificChatGroup(int chatId, int dogId)
        //{
        //    ChatHubTracker.JoinChat(dogId, chatId);
        //    await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        //}

        //public async Task LeaveSpecificChatGroup(int chatId, int dogId)
        //{
        //    ChatHubTracker.LeaveChat(dogId, chatId);
        //    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        //}

        public async Task JoinDogChatsGroup(int dogId)
        {
            // ✅ Check if connected user owns the dog they're trying to subscribe to
            if (!Context.User.Claims.Any(c => c.Type == "DogId" && c.Value == dogId.ToString()))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized dog access."); // ✅ Prevent unauthorized group access
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"DogChats_{dogId}");
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
        }

        public async Task JoinSpecificChatGroup(int chatId, int dogId)
        {
            ChatHubTracker.JoinChat(dogId, chatId);

            // ✅ Security: validate dog ownership before joining chat group
            if (!Context.User.Claims.Any(c => c.Type == "DogId" && c.Value == dogId.ToString()))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized dog access.");
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        }

        public async Task LeaveSpecificChatGroup(int chatId, int dogId)
        {
            ChatHubTracker.LeaveChat(dogId, chatId);

            // ✅ Same security enforcement as join
            if (!Context.User.Claims.Any(c => c.Type == "DogId" && c.Value == dogId.ToString()))
            {
                await Clients.Caller.SendAsync("Error", "Unauthorized dog access.");
                return;
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Chat_{chatId}");
        }

    }
}
