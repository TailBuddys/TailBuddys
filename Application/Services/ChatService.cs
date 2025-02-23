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
            // צריך להכליל בפונקציה הזו גם עדכון של נוטיפיקיישן סרוויס
            try
            {
                Match? myMatch = _matchRepository.GetAllMatchesAsSenderDogDb(chat.SenderDogId)
                    .Result.FirstOrDefault(m => m.ReciverDogId == chat.ReciverDogId);

                Match? foreignMatch = _matchRepository.GetAllMatchesAsReciverDogDb(chat.SenderDogId)
                    .Result.FirstOrDefault(m => m.SenderDogId == chat.ReciverDogId);

                if (myMatch != null && foreignMatch != null && myMatch.IsMatch && foreignMatch.IsMatch)
                {
                    Chat? foreignChat = _chatRepository.GetAllDogChatsDb(chat.SenderDogId)
                    .Result.FirstOrDefault(c => c.SenderDogId == chat.ReciverDogId || c.ReciverDogId == chat.ReciverDogId);

                    if (foreignChat == null)
                    {
                        return await _chatRepository.CreateChatDb(chat);
                    }

                    if (chat.Messages.Count > 0)
                    {
                        chat.Messages.First().ChatID = foreignChat.Id;
                        await _chatRepository.AddMessageToChatDb(chat.Messages.First());
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
            try
            {
                return await _chatRepository.GetAllDogChatsDb(dogId);

            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Chat>();
            }
        }
        public async Task<Chat?> GetChatById(string chatId)
        {
            try
            {
                return await _chatRepository.GetChatByIdDb(chatId);

            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        //צריך לבדוק מה לעשות אם יוזר מוחק את הצאט 
        public async Task<Chat?> UpdateChat(string chatId, Chat chat)
        {
            try
            {
                return await _chatRepository.UpdateChatDb(chatId, chat);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<Chat?> DeleteChat(string chatId)
        {
            try
            {
                return await _chatRepository.DeleteChatDb(chatId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        // לוודא שהכלב השולח קיים בצ'אט איי די ולבדוק האם הוא הסנדר איי די
        public async Task<Message?> AddMessageToChat(Message message)
        {
            return new Message();
        }
        public async Task<List<Message>> GetMessagesByChatId(string chatId)
        {
            try
            {
                return await _chatRepository.GetMessagesByChatIdDb(chatId);

            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<Message>();
            }

        }
        public async Task<Message?> MarkMessageAsRead(string messageId)
        {
            return new Message();

        }

    }
}
