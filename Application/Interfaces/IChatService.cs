using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IChatService
    {
        public Task<Chat?> CreateChat(Chat chat);
        public Task<List<Chat>> GetAllDogChats(int dogId);
        public Task<Chat?> GetChatById(int chatId);
        public Task<Chat?> UpdateChat(int chatId, Chat chat);
        public Task<Chat?> DeleteChat(int chatId);
        public Task<Message?> AddMessageToChat(Message message);
        public Task<List<Message>> GetMessagesByChatId(int chatId);
        public Task<Message?> MarkMessageAsRead(int messageId);
    }
}
