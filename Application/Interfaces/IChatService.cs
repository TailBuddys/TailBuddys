using TailBuddys.Core.DTO;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.DTO;

namespace TailBuddys.Application.Interfaces
{
    public interface IChatService
    {
        public Task<Chat?> CreateChat(Chat chat);
        public Task<List<ChatDTO>> GetAllDogChats(int dogId);
        public Task<FullChatDTO?> GetChatById(int chatId);
        public Task<Chat?> GetChatDetailsById(int chatId);
        public Task<Chat?> UpdateChat(int chatId, Chat chat);
        public Task<Chat?> DeleteChat(int chatId);
        public Task<Message?> AddMessageToChat(Chat chat, Message message);
        public Task<List<Message>> GetMessagesByChatId(int chatId);
        public Task<int> MarkAllMessagesAsRead(int chatId, int currentDogId);
        //public Task SendMessage(int chatId, int senderDogId, int receiverDogId, string message);
    }
}
