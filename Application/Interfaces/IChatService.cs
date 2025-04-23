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
        public Task<ChatDTO?> UpdateChat(int chatId, bool isArchive, int clientDogId);
        public Task<Chat?> DeleteChat(int chatId);
        public Task<Message?> AddMessageToChat(Chat chat, Message message);
        public Task<int> MarkAllMessagesAsRead(int chatId, int currentDogId);

    }
}
