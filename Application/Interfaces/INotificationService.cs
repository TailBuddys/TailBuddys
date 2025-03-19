using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface INotificationService
    {
        //CHAT//
        public Task<ChatNotification?> CreateOrUpdateChatNotification(int chatId, int dogId);
        public Task<List<ChatNotification>> GetAllDogChatsNotifications(int dogId);
        public Task<ChatNotification?> GetChatNotificationsById(int chatId, int dogId);
        public Task<ChatNotification?> DeleteChatNotifications(int chatId, int dogId);
        //MATCH//
        public Task<MatchNotification?> CreateMatchNotification(int dogId, int matchId);
        public Task<List<MatchNotification>> GetDogAllMatchesNotifications(int dogId);
        public Task<List<MatchNotification>> DeleteMatchesNotifications(int dogId);
    }
}
