using Microsoft.EntityFrameworkCore;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Infrastructure.Data;

namespace TailBuddys.Infrastructure.Services
{


    public class NotificationRepository : INotificationRepository
    {

        private readonly TailBuddysContext _context;

        public NotificationRepository(TailBuddysContext context)
        {
            _context = context;
        }

        //CHAT//
        public async Task<ChatNotification?> CreateChatNotificationDB(int chatId, int dogId)
        {
            try
            {
                ChatNotification newChat = new ChatNotification
                {
                    ChatId = chatId,
                    DogId = dogId,
                    UnreadCount = 0,
                };

                _context.ChatNotifications.Add(newChat);
                await _context.SaveChangesAsync();
                return newChat;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<List<ChatNotification>> GetAllDogChatsNotificationsDB(int dogId)
        {
            try
            {
                List<ChatNotification> chatNotifyList = await _context
                    .ChatNotifications.Where(c => c.DogId == dogId).ToListAsync();

                if (chatNotifyList.Count == 0)
                {
                    return new List<ChatNotification>();
                }
                return chatNotifyList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<ChatNotification>();
            }
        }
        public async Task<ChatNotification?> GetChatNotificationsByIdDB(int chatId, int dogId)
        {
            try
            {
                ChatNotification? chatNotify = await _context
                    .ChatNotifications.FirstOrDefaultAsync(c => c.ChatId == chatId && c.DogId == dogId);
                return chatNotify;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<ChatNotification?> UpdateChatNotificationsByIdDB(int chatId, int dogId)
        {
            try
            {
                ChatNotification? notifyToUpdate = await _context
                    .ChatNotifications.FirstOrDefaultAsync(c => c.ChatId == chatId && c.DogId == dogId);
                if (notifyToUpdate == null) return null;

                notifyToUpdate.UnreadCount++;
                await _context.SaveChangesAsync();
                return notifyToUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<ChatNotification?> DeleteChatNotificationsDB(int chatId, int dogId)
        {
            try
            {
                ChatNotification? chatNotifyToDelete = await _context
                    .ChatNotifications.FirstOrDefaultAsync(c => c.ChatId == chatId && c.DogId == dogId);
                if (chatNotifyToDelete == null) return null;

                _context.ChatNotifications.Remove(chatNotifyToDelete);
                await _context.SaveChangesAsync();
                return chatNotifyToDelete;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        //MATCH//
        public async Task<MatchNotification?> CreateMatchNotificationDB(int dogId, int matchId)
        {
            try
            {
                MatchNotification newMatch = new MatchNotification
                {
                    DogId = dogId,
                    MatchId = matchId
                };

                _context.MatchNotification.Add(newMatch);
                await _context.SaveChangesAsync();
                return newMatch;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<List<MatchNotification>> GetDogAllMatchesNotificationsDB(int dogId)
        {
            try
            {
                List<MatchNotification> matchNotifyList = await _context
                    .MatchNotification.Where(c => c.DogId == dogId).ToListAsync();

                if (matchNotifyList.Count == 0)
                {
                    return new List<MatchNotification>();
                }
                return matchNotifyList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<MatchNotification>();
            }
        }
        public async Task<List<MatchNotification>> DeleteMatchesNotificationsDB(int dogId)
        {
            try
            {
                List<MatchNotification> matchNotifyToDelete = await _context
                    .MatchNotification.Where(c => c.DogId == dogId).ToListAsync();

                if (matchNotifyToDelete == null) return new List<MatchNotification>();

                foreach (MatchNotification matchNotify in matchNotifyToDelete)
                {
                    _context.MatchNotification.Remove(matchNotify);
                }
                await _context.SaveChangesAsync();
                return matchNotifyToDelete;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return new List<MatchNotification>();
            }
        }
    }
}
