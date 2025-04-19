using Microsoft.AspNetCore.SignalR;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.DTO;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.DTO;
using TailBuddys.Hubs;

namespace TailBuddys.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly INotificationService _notificationService;
        private readonly IOpenAiService _openAiService;

        public ChatService(IChatRepository chatRepository, IMatchRepository matchRepository, IUserRepository userRepository, IHubContext<ChatHub> chatHubContext, INotificationService notificationService, IOpenAiService openAiService)
        {
            _chatRepository = chatRepository;
            _matchRepository = matchRepository;
            _chatHubContext = chatHubContext;
            _notificationService = notificationService;
            _openAiService = openAiService;
            _userRepository = userRepository;
        }

        public async Task<Chat?> CreateChat(Chat chat)
        {
            // צריך להכליל בפונקציה הזו גם עדכון של נוטיפיקיישן סרוויס
            // להגדיר שליחת הודעה ראשונה בעת פתיחת צ'אט חדש
            try
            {
                //GPT review 

                var senderMatches = await _matchRepository.GetAllMatchesAsSenderDogDb(chat.SenderDogId);
                Match? myMatch = senderMatches.FirstOrDefault(m => m.ReceiverDogId == chat.ReceiverDogId);

                var receiverMatches = await _matchRepository.GetAllMatchesAsReceiverDogDb(chat.SenderDogId);
                Match? foreignMatch = receiverMatches.FirstOrDefault(m => m.SenderDogId == chat.ReceiverDogId);

                //Match? myMatch = _matchRepository.GetAllMatchesAsSenderDogDb(chat.SenderDogId)
                //    .Result.FirstOrDefault(m => m.ReceiverDogId == chat.ReceiverDogId);

                //Match? foreignMatch = _matchRepository.GetAllMatchesAsReceiverDogDb(chat.SenderDogId)
                //    .Result.FirstOrDefault(m => m.SenderDogId == chat.ReceiverDogId);

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
        public async Task<List<ChatDTO>> GetAllDogChats(int dogId)
        {
            try
            {
                List<Chat> chats = await _chatRepository.GetAllDogChatsDb(dogId);
                List<ChatDTO> ChatsToReturn = new List<ChatDTO>();
                foreach (Chat chat in chats)
                {
                    Dog? receiverDog = (dogId == chat.SenderDogId) ? chat.ReceiverDog : chat.SenderDog;
                    if (receiverDog != null)
                    {
                        ChatsToReturn.Add(new ChatDTO
                        {
                            Id = chat.Id,
                            Dog = new UserDogDTO
                            {
                                Id = receiverDog.Id,
                                Name = receiverDog.Name,
                                ImageUrl = receiverDog.Images.FirstOrDefault(i => i.Order == 0)?.Url
                            },
                            LastMessage = chat.Messages.LastOrDefault()
                        });
                    }
                }
                return ChatsToReturn;

            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return new List<ChatDTO>();
            }
        }
        public async Task<FullChatDTO?> GetChatById(int chatId)
        {
            try
            {
                Chat? chat = await _chatRepository.GetChatByIdDb(chatId);
                if (chat == null) return null;
                return new FullChatDTO
                {
                    Id = chat.Id,
                    SenderDog = new UserDogDTO
                    {
                        Id = chat.SenderDogId,
                        Name = chat.SenderDog?.Name,
                        ImageUrl = chat.SenderDog?.Images.FirstOrDefault(i => i.Order == 0)?.Url
                    },
                    ReceiverDog = new UserDogDTO
                    {
                        Id = chat.ReceiverDogId,
                        Name = chat.ReceiverDog?.Name,
                        ImageUrl = chat.ReceiverDog?.Images.FirstOrDefault(i => i.Order == 0)?.Url
                    },
                    Messages = chat.Messages.ToList()
                };
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
        public async Task<Message?> AddMessageToChat(Chat chatToUpdate, Message message)
        {
            try
            {

                Dog? receiverDog = message.SenderDogId == chatToUpdate.SenderDogId ? chatToUpdate.ReceiverDog : chatToUpdate.SenderDog;
                if (receiverDog == null) return null;
                Dog? senderDog = chatToUpdate.SenderDogId == receiverDog.Id ? chatToUpdate.ReceiverDog : chatToUpdate.SenderDog;
                if (senderDog == null) return null;
                await _chatRepository.AddMessageToChatDb(message);

                if (receiverDog.IsBot == true && senderDog.IsBot == false)
                {
                    User? user = await _userRepository.GetUserByIdDb(receiverDog.UserId);
                    User? otherUser = await _userRepository.GetUserByIdDb(senderDog.UserId);
                    if (user != null && otherUser != null)
                    {
                        string aiResponse = await _openAiService.GetDogChatBotReplyAsync(user, receiverDog, otherUser, senderDog, chatToUpdate);
                        if (!string.IsNullOrWhiteSpace(aiResponse))
                        {
                            await _chatRepository.AddMessageToChatDb(new Message
                            {
                                SenderDogId = receiverDog.Id,
                                ChatID = chatToUpdate.Id,
                                Content = aiResponse,
                            });
                        }
                    }

                }
                return message;
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
            try
            {
                return await _chatRepository.MarkMessageAsReadDb(messageId);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }

        }

        public async Task<Chat?> GetChatDetailsById(int chatId)
        {
            try
            {
                Chat? chat = await _chatRepository.GetChatByIdDb(chatId);
                if (chat == null) return null;

                return chat;
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public async Task SendMessage(int chatId, int senderDogId, int receiverDogId, string message)
        {
            // Send message to specific chat group
            await _chatHubContext.Clients.Group($"Chat_{chatId}").SendAsync("ReceiveMessage", new { chatId, senderDogId, message });

            // Check if receiver is in active chat
            bool isReceiverInChat = ChatHubTracker.IsDogInChat(receiverDogId, chatId);

            if (!isReceiverInChat)
            {
                // Update notification count in DB
                await _notificationService.CreateOrUpdateChatNotification(chatId, receiverDogId);

                // Notify receiver's chat list UI
                await _chatHubContext.Clients.Group($"DogChats_{receiverDogId}").SendAsync("ReceiveChatNotification", new { chatId });
            }
        }
    }
}
