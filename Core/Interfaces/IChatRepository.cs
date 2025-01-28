using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IChatRepository
    {
        public Task<Chat?> CreateChat(string id);
        public Task<List<Chat?>> GetAllChats(string id);
        public Task<Chat?> GetChatById(string id);
        public Task<Chat?> UpdateChat(string id);
        public Task<Chat?> DeleteChat(string id);
    }
}
