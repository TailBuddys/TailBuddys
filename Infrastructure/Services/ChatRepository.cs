using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Infrastructure.Data;

namespace TailBuddys.Infrastructure.Services
{
    public class ChatRepository : IChatRepository
    {
        private readonly TailBuddysContext _context;
        public ChatRepository(TailBuddysContext context)
        {
            _context = context;
        }
        public async Task<Chat?> CreateChat(string fromDogId, string toDogId)
        {
            return new Chat();
        }
        public async Task<List<Chat?>> GetAllDogChats(string dogId)
        {
            return new List<Chat>();

        }
        public async Task<Chat?> GetChatById(string chatId)
        {
            return new Chat();

        }
        public async Task<Chat?> UpdateChat(string chatId, Chat chat)
        {
            return new Chat();

        }
        public async Task<Chat?> DeleteChat(string chatId)
        {
            return new Chat();

        }
        public async Task<Chat?> AddMessageToChat(string chatId, Message message)
        {
            return new Chat();

        }
        public async Task<List<Message>> GetMessagesByChatId(string chatId)
        {
            return new List<Message>();

        }
        public async Task<Message?> MarkMessageAsRead(string messageId)
        {
            return new Message();

        }
    }
}
