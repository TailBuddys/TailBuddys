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
            // להגדיר שליחת הודעה ראשונה בעת פתיחת צ'אט חדש
            try
            {
                Match? myMatch = _matchRepository.GetAllMatchesAsSenderDogDb(chat.SenderDogId)
                    .Result.FirstOrDefault(m => m.ReceiverDogId == chat.ReceiverDogId);

                Match? foreignMatch = _matchRepository.GetAllMatchesAsReceiverDogDb(chat.SenderDogId)
                    .Result.FirstOrDefault(m => m.SenderDogId == chat.ReceiverDogId);

                if (myMatch != null && foreignMatch != null && myMatch.IsMatch && foreignMatch.IsMatch)
                {
                    Chat? foreignChat = _chatRepository.GetAllDogChatsDb(chat.SenderDogId)
                    .Result.FirstOrDefault(c => c.SenderDogId == chat.ReceiverDogId || c.ReceiverDogId == chat.ReceiverDogId);

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
        public async Task<List<Chat>> GetAllDogChats(int dogId)
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
        public async Task<Chat?> GetChatById(int chatId)
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
        public async Task<Chat?> UpdateChat(int chatId, Chat chat)
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
        public async Task<Chat?> DeleteChat(int chatId)
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
            try
            {
                return await _chatRepository.AddMessageToChatDb(message);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
        public async Task<List<Message>> GetMessagesByChatId(int chatId)
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
        // ליישם את הפונקציה
        public async Task<Message?> MarkMessageAsRead(int messageId)
        {
            return new Message();
        }

    }
}
