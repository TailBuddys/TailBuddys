using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Services
{
    public class NotificationService : INotificationService
    {

        private readonly INotificationRepository _notificationRepository;
        private readonly IChatRepository _chatRepository;

        public NotificationService(INotificationRepository notificationRepository, IChatRepository chatRepository)
        {
            _notificationRepository = notificationRepository;
            _chatRepository = chatRepository;
        }

        //CHAT//
        //public async Task<ChatNotification?> CreateOrUpdateChatNotification(int chatId, int dogId)
        //{
        //    ChatNotification? chatNotify = await _notificationRepository.GetChatNotificationsByIdDB(chatId, dogId);


        //    if (chatNotify == null)
        //    {
        //        chatNotify = new ChatNotification
        //        {
        //            DogId = dogId,
        //            ChatId = chatId,
        //            UnreadCount = 1
        //        };
        //        return await _notificationRepository.CreateChatNotificationDB(chatNotify.ChatId, chatNotify.DogId);
        //    }

        //    return await _notificationRepository.UpdateChatNotificationsByIdDB(chatId, dogId);
        //}

        public async Task<ChatNotification?> CreateOrUpdateChatNotification(int chatId, int dogId)
        {
            var existing = await _notificationRepository.GetChatNotificationsByIdDB(chatId, dogId);

            if (existing == null)
            {
                return await _notificationRepository.CreateChatNotificationDB(chatId, dogId);
            }
            else
            {
                return await _notificationRepository.UpdateChatNotificationsByIdDB(chatId, dogId);
            }
        }

        public async Task<List<ChatNotification>> GetAllDogChatsNotifications(int dogId)
        {
            return await _notificationRepository.GetAllDogChatsNotificationsDB(dogId);
        }

        public async Task<ChatNotification?> GetChatNotificationsById(int chatId, int dogId)
        {
            ChatNotification? chatNotifications = await _notificationRepository.GetChatNotificationsByIdDB(chatId, dogId);

            if (chatNotifications != null)
            {
                return chatNotifications;
            }
            return null;
        }

        public async Task<ChatNotification?> DeleteChatNotifications(int chatId, int dogId)
        {
            var chatNotification = await _notificationRepository.DeleteChatNotificationsDB(chatId, dogId);
            if (chatNotification != null)
            {
                await _chatRepository.MarkAllMessagesAsReadDb(chatId,dogId);
            }
            return chatNotification;
        }

        //MATCH//

        public async Task<MatchNotification?> CreateMatchNotification(int dogId, int matchId)
        {
            return await _notificationRepository.CreateMatchNotificationDB(dogId, matchId);
        }

        public async Task<List<MatchNotification>> GetDogAllMatchesNotifications(int dogId)
        {
            return await _notificationRepository.GetDogAllMatchesNotificationsDB(dogId);
        }

        public async Task<List<MatchNotification>> DeleteMatchesNotifications(int dogId)
        {
            return await _notificationRepository.DeleteMatchesNotificationsDB(dogId);
        }
    }
}
