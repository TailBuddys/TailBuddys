using TailBuddys.Core.Models;

namespace TailBuddys.Application.Interfaces
{
    public interface IChatService
    {
        public Task<Chat?> CreateChat(Chat chat);
        public Task<List<Chat>> GetAllDogChats(string dogId);
        public Task<Chat?> GetChatById(string chatId);
        public Task<Chat?> UpdateChat(string chatId, Chat chat);
        public Task<Chat?> DeleteChat(string chatId);
        public Task<Message?> AddMessageToChat(Message message);
        public Task<List<Message>> GetMessagesByChatId(string chatId);
        public Task<Message?> MarkMessageAsRead(string messageId);
    }
}
