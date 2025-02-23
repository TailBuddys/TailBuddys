using Microsoft.AspNetCore.SignalR;

namespace TailBuddys.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
        // יישום של סרוויס האב שמדבר ישירות עם הסרוויס של נוטיפיקציות בבאק ואת הפרונט בהתאם
        //private readonly NotificationService _notificationService;

        //public NotificationHub(NotificationService notificationService)
        //{
        //    _notificationService = notificationService;
        //}

        //public async Task JoinDogGroup(string dogId)
        //{
        //    await Groups.AddToGroupAsync(Context.ConnectionId, dogId);
        //}

        //public async Task NotifyNewEvent(string receiverDogId, string eventType)
        //{
        //    var unreadCount = await _notificationService.AddNotification(receiverDogId);
        //    await Clients.Group(receiverDogId).SendAsync("ReceiveNotification", eventType, unreadCount);
        //}

        //public async Task MarkAsRead(string dogId)
        //{
        //    await _notificationService.MarkAsRead(dogId);
        //    await Clients.Group(dogId).SendAsync("NotificationRead", dogId);
        //}
    }
}
