using Microsoft.AspNetCore.SignalR;
using TailBuddys.Application.Interfaces;
using TailBuddys.Core.DTO;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Core.Models.DTO;
using TailBuddys.Hubs;
using TailBuddys.Hubs.HubInterfaces;

namespace TailBuddys.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<ChatHub> _chatHubContext;
        private readonly IHubContext<NotificationHub> _matchHubContext;
        private readonly INotificationService _notificationService;
        private readonly IOpenAiService _openAiService;
        private readonly IDogConnectionTracker _tracker;


        public ChatService(
            IChatRepository chatRepository,
            IMatchRepository matchRepository,
            IUserRepository userRepository,
            IHubContext<ChatHub> chatHubContext,
            IHubContext<NotificationHub> matchHubContext,
            INotificationService notificationService,
            IOpenAiService openAiService,
            IDogConnectionTracker tracker)
        {
            _chatRepository = chatRepository;
            _matchRepository = matchRepository;
            _chatHubContext = chatHubContext;
            _matchHubContext = matchHubContext;
            _notificationService = notificationService;
            _openAiService = openAiService;
            _userRepository = userRepository;
            _tracker = tracker;
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
                            if (_tracker.IsDogInChatsGroup(chat.ReceiverDogId))
                            {
                                await _chatHubContext.Clients.Group($"DogChats_{chat.ReceiverDogId}")
                                    .SendAsync("ReceiveChatNotification", newChat);
                            }
                            else
                            {
                                await _notificationService.CreateOrUpdateChatNotification(newChat.Id, chat.ReceiverDogId);
                            }
                            if (_tracker.IsDogInChatsGroup(chat.SenderDogId))
                            {
                                await _chatHubContext.Clients.Group($"DogChats_{chat.SenderDogId}")
                                    .SendAsync("ReceiveChatNotification", newChat);
                            }
                            else
                            {
                                await _notificationService.CreateOrUpdateChatNotification(newChat.Id, chat.SenderDogId);
                            }
                            // for matches
                            if (_tracker.IsDogInMatchGroup(chat.ReceiverDogId))
                            {
                                await _matchHubContext.Clients.Group(chat.ReceiverDogId.ToString()).SendAsync("ReceiveNewMatch", 0);
                            }
                            if (_tracker.IsDogInMatchGroup(chat.SenderDogId))
                            {
                                await _matchHubContext.Clients.Group(chat.SenderDogId.ToString()).SendAsync("ReceiveNewMatch", 0);
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
                Chat? deletedChat = await _chatRepository.DeleteChatDb(chatId);
                if (deletedChat != null)
                {
                    if (_tracker.IsDogInChatsGroup(deletedChat.ReceiverDogId))
                    {
                        await _chatHubContext.Clients.Group($"DogChats_{deletedChat.ReceiverDogId}")
                            .SendAsync("ReceiveChatNotification", deletedChat);
                    }
                    else
                    {
                        await _notificationService.CreateOrUpdateChatNotification(chatId, deletedChat.ReceiverDogId);
                    }
                    if (_tracker.IsDogInChatsGroup(deletedChat.SenderDogId))
                    {
                        await _chatHubContext.Clients.Group($"DogChats_{deletedChat.SenderDogId}")
                            .SendAsync("ReceiveChatNotification", deletedChat);
                    }
                    else
                    {
                        await _notificationService.CreateOrUpdateChatNotification(chatId, deletedChat.SenderDogId);
                    }
                    // for matches
                    if (_tracker.IsDogInMatchGroup(deletedChat.ReceiverDogId))
                    {
                        await _matchHubContext.Clients.Group(deletedChat.ReceiverDogId.ToString()).SendAsync("ReceiveNewMatch", 0);
                    }
                    if (_tracker.IsDogInMatchGroup(deletedChat.SenderDogId))
                    {
                        await _matchHubContext.Clients.Group(deletedChat.SenderDogId.ToString()).SendAsync("ReceiveNewMatch", 0);
                    }
                }
                return deletedChat;
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
                bool isReceiverInChat = _tracker.IsDogInSpecificChat(receiverDog.Id, chatToUpdate.Id);

                message.IsRead = isReceiverInChat;

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
                            Message botMessage = new Message
                            {
                                SenderDogId = receiverDog.Id,
                                ChatID = chatToUpdate.Id,
                                Content = aiResponse,
                                IsRead = _tracker.IsDogInSpecificChat(senderDog.Id, chatToUpdate.Id)
                            };
                            await _chatRepository.AddMessageToChatDb(botMessage);
                            bool isSenderInChat = _tracker.IsDogInSpecificChat(senderDog.Id, chatToUpdate.Id);
                            if (isSenderInChat)
                            {
                                // Send message directly if sender is in chat
                                await _chatHubContext.Clients.Group($"Chat_{chatToUpdate.Id}")
                                    .SendAsync("ReceiveChatNotification", new
                                    {
                                        chatId = chatToUpdate.Id,
                                        senderDogId = botMessage.SenderDogId,
                                        message = botMessage.Content
                                    });
                            }
                            else
                            {
                                // Send notification if sender is not in chat
                                var chatNotify = await _notificationService.CreateOrUpdateChatNotification(chatToUpdate.Id, senderDog.Id);
                                await _chatHubContext.Clients.Group($"DogChats_{senderDog.Id}")
                                    .SendAsync("ReceiveChatNotification", chatNotify);
                            }
                        }
                    }

                }
                if (isReceiverInChat)
                {
                    await _chatHubContext.Clients.Group($"Chat_{chatToUpdate.Id}")
                        .SendAsync("ReceiveChatNotification", new { chatId = chatToUpdate.Id, senderDogId = message.SenderDogId, message = message.Content });

                }
                else if (!receiverDog.IsBot == true)
                {
                    var chatNotify = await _notificationService.CreateOrUpdateChatNotification(chatToUpdate.Id, receiverDog.Id);

                    await _chatHubContext.Clients.Group($"DogChats_{receiverDog.Id}")
                        .SendAsync("ReceiveChatNotification", chatNotify);
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

        public async Task<int> MarkAllMessagesAsRead(int chatId, int currentDogId)
        {
            try
            {
                return await _chatRepository.MarkAllMessagesAsReadDb(chatId, currentDogId);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
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

        //public async Task SendMessage(int chatId, int senderDogId, int receiverDogId, string message)
        //{
        //    await _chatHubContext.Clients.Group($"Chat_{chatId}")
        //        .SendAsync("ReceiveChatNotification", new
        //        {
        //            chatId,
        //            senderDogId,
        //            message
        //        });

        //    if (_tracker.IsDogInSpecificChat(receiverDogId, chatId))
        //    {
        //        return;
        //    }

        //    var chatNotify = await _notificationService.CreateOrUpdateChatNotification(chatId, receiverDogId);
        //    if (_tracker.IsDogInChatsGroup(receiverDogId))
        //    {
        //        await _chatHubContext.Clients.Group($"DogChats_{receiverDogId}")
        //            .SendAsync("ReceiveChatNotification", chatNotify);
        //    }
        //}
    }
}
