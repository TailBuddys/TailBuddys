using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IChatRepository
    {
        public Task<Chat?> CreateChatDb(Chat chat);
        public Task<List<Chat>> GetAllDogChatsDb(string dogId);
        public Task<Chat?> GetChatByIdDb(string chatId);
        public Task<Chat?> UpdateChatDb(string chatId, Chat chat);
        public Task<Chat?> DeleteChatDb(string chatId);
        public Task<Message?> AddMessageToChatDb(Message message);
        public Task<List<Message>> GetMessagesByChatIdDb(string chatId);
        public Task<Message?> MarkMessageAsReadDb(string messageId);
    }
}
