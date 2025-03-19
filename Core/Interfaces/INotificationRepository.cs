using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface INotificationRepository
    {
        //CHAT//
        public Task<ChatNotification?> CreateChatNotificationDB(int chatId, int dogId);
        public Task<List<ChatNotification>> GetAllDogChatsNotificationsDB(int dogId);
        public Task<ChatNotification?> GetChatNotificationsByIdDB(int chatId, int dogId);
        public Task<ChatNotification?> UpdateChatNotificationsByIdDB(int chatId, int dogId);
        public Task<ChatNotification?> DeleteChatNotificationsDB(int chatId, int dogId);
        //MATCH//
        public Task<MatchNotification?> CreateMatchNotificationDB(int dogId, int matchId);
        public Task<List<MatchNotification>> GetDogAllMatchesNotificationsDB(int dogId);
        public Task<List<MatchNotification>> DeleteMatchesNotificationsDB(int dogId);
    }
}
