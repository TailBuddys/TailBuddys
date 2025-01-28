using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IChatRepository
    {
        public Task<Chat?> CreateChat(string fromDogId, string toDogId);
        public Task<List<Chat?>> GetAllDogChats(string dogId);
        public Task<Chat?> GetChatById(string chatId);
        public Task<Chat?> UpdateChat(string chatId, Chat chat);
        public Task<Chat?> DeleteChat(string chatId);
        public Task<Chat?> AddMessageToChat(string chatId, Message message);
        public Task<List<Message>> GetMessagesByChatId(string chatId);
        public Task<Message?> MarkMessageAsRead(string messageId);
    }
}
