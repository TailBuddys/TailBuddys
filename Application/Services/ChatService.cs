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
                var senderMatches = await _matchRepository.GetAllMatchesAsSenderDogDb(chat.SenderDogId);
                Match? myMatch = senderMatches.FirstOrDefault(m => m.ReceiverDogId == chat.ReceiverDogId);

                var receiverMatches = await _matchRepository.GetAllMatchesAsReceiverDogDb(chat.SenderDogId);
                Match? foreignMatch = receiverMatches.FirstOrDefault(m => m.SenderDogId == chat.ReceiverDogId);

                if (myMatch != null && foreignMatch != null && myMatch.IsMatch && foreignMatch.IsMatch)
                {
                    Chat? foreignChat = _chatRepository.GetAllDogChatsDb(chat.SenderDogId)
                    .Result.FirstOrDefault(c => c.SenderDogId == chat.ReceiverDogId || c.ReceiverDogId == chat.ReceiverDogId);
                    if (foreignChat == null)
                    {
                        Chat? newChat = await _chatRepository.CreateChatDb(chat);
                        if (newChat != null)
                        {
                            bool isReceiverInChatList = ChatHubTracker.IsDogInChat(chat.ReceiverDogId, newChat.Id);
                            if (isReceiverInChatList)
                            {
                                await _chatHubContext.Clients.Group($"DogChats_{chat.ReceiverDogId}")
                                    .SendAsync("ReceiveNewChat", newChat.Id);
                            }
                            else
                            {
                                await _notificationService.CreateOrUpdateChatNotification(newChat.Id, chat.ReceiverDogId);
                            }
                            return newChat;
                        }
                        else
                        {
                            return null;
                        }
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
                        Message? lastMessage = chat.Messages
                            .OrderByDescending(m => m.CreatedAt)
                            .FirstOrDefault();

                        ChatsToReturn.Add(new ChatDTO
                        {
                            Id = chat.Id,
                            Dog = new UserDogDTO
                            {
                                Id = receiverDog.Id,
                                Name = receiverDog.Name,
                                ImageUrl = receiverDog.Images.FirstOrDefault(i => i.Order == 0)?.Url
                            },
                            LastMessage = lastMessage
                        });
                    }
                }

                return ChatsToReturn
                    .OrderByDescending(c => c.LastMessage?.CreatedAt)
                    .ToList();
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
                bool isReceiverInChat = ChatHubTracker.IsDogInChat(receiverDog.Id, chatToUpdate.Id);
                if (isReceiverInChat)
                {
                    await _chatHubContext.Clients.Group($"Chat_{chatToUpdate.Id}")
                        .SendAsync("ReceiveMessage", new { chatId = chatToUpdate.Id, senderDogId = message.SenderDogId, message = message.Content });
                }
                else
                {
                    await _notificationService.CreateOrUpdateChatNotification(chatToUpdate.Id, receiverDog.Id);

                    await _chatHubContext.Clients.Group($"DogChats_{receiverDog.Id}")
                        .SendAsync("ReceiveChatNotification", new { chatId = chatToUpdate.Id });
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
