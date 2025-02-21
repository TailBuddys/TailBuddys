using TailBuddys.Application.Interfaces;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;

namespace TailBuddys.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMatchRepository _matchRepository;
        public ChatService(IChatRepository chatRepository, IMatchRepository matchRepository)
        {
            _chatRepository = chatRepository;
            _matchRepository = matchRepository;
        }

        public async Task<Chat?> CreateChat(Chat chat)
        {
            try
            {
                Match? myMatch = _matchRepository.GetAllMatchesAsSenderDogDb(chat.SenderDogId)
                    .Result.FirstOrDefault(m => m.SenderDogId == chat.ReciverDogId);

                Match? foreignMatch = _matchRepository.GetAllMatchesAsSenderDogDb(chat.ReciverDogId)
                    .Result.FirstOrDefault(m => m.SenderDogId == chat.SenderDogId);

                if (myMatch != null && foreignMatch != null && myMatch.IsMatch == foreignMatch.IsMatch)
                {
                    Chat? foreignChat = _chatRepository.GetAllDogChatsDb(chat.SenderDogId)
                    .Result.FirstOrDefault(c => c.SenderDogId == chat.SenderDogId || c.ReciverDogId == chat.ReciverDogId);

                    if (foreignChat == null)
                    {
                        return await _chatRepository.CreateChatDb(chat);
                    }

                    if (chat.Messages.Count > 0)
                    {
                        foreignChat.Messages.Add(chat.Messages.First());
                    }

                    return foreignChat;
                }

                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Chat>> GetAllDogChats(string dogId)
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
        public async Task<Message?> AddMessageToChat(Message message)
        {
            return new Message();
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
