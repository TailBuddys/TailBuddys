using Microsoft.EntityFrameworkCore;
using TailBuddys.Application.Services;
using TailBuddys.Core.Interfaces;
using TailBuddys.Core.Models;
using TailBuddys.Infrastructure.Data;

namespace TailBuddys.Infrastructure.Services
{
    public class ChatRepository : IChatRepository
    {
        private readonly TailBuddysContext _context;
        private readonly ILogger<ChatRepository> _logger;

        public ChatRepository(TailBuddysContext context, ILogger<ChatRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Chat?> CreateChatDb(Chat chat)
        {
            try
            {
                chat.SenderDogArchive = false;
                chat.ReceiverDogArchive = false;
                _context.Chats.Add(chat);
                await _context.SaveChangesAsync();
                return chat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred creating new chat.");
                return null;
            }
        }
        public async Task<List<Chat>> GetAllDogChatsDb(int dogId)
        {
            try
            {
                List<Chat> list = await _context.Chats
                    .Include(c => c.SenderDog!).ThenInclude(d => d.Images)
                    .Include(c => c.ReceiverDog!).ThenInclude(d => d.Images)
                    .Include(c => c.Messages)
                    .Where(c => c.SenderDogId == dogId || c.ReceiverDogId == dogId).ToListAsync();
                return list;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred get all dog new chat.");
                return new List<Chat>();
            }
        }
        public async Task<Chat?> GetChatByIdDb(int chatId)
        {
            try
            {
                Chat? chat = await _context.Chats
                    .Include(c => c.SenderDog!).ThenInclude(d => d.Images)
                    .Include(c => c.ReceiverDog!).ThenInclude(d => d.Images)
                    .Include(c => c.Messages)
                    .FirstOrDefaultAsync(c => c.Id == chatId);
                return chat;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred getting chat by id.");
                return null;
            }
        }
        public async Task<Chat?> UpdateChatDb(int chatId, bool isArchive, int clientDogId)
        {
            try
            {
                Chat? chatToUpdate = await _context.Chats
                    .Include(c => c.SenderDog!).ThenInclude(d => d.Images)
                    .Include(c => c.ReceiverDog!).ThenInclude(d => d.Images)
                    .Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == chatId);
                if (chatToUpdate == null)
                {
                    return null;
                }

                if (chatToUpdate.SenderDogId == clientDogId) 
                {
                    chatToUpdate.SenderDogArchive = isArchive;
                }
                else 
                {
                    chatToUpdate.ReceiverDogArchive = isArchive; 
                }

                _context.Chats.Update(chatToUpdate);
                await _context.SaveChangesAsync();
                return chatToUpdate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred updating chat.");
                return null;
            }

        }
        public async Task<Chat?> DeleteChatDb(int chatId)
        {
            try
            {
                Chat? chatToRemove = await _context.Chats
                    .Include(c => c.Messages)
                    .FirstOrDefaultAsync(c => c.Id == chatId);

                if (chatToRemove == null)
                {
                    return null;
                }

                _context.Chats.Remove(chatToRemove);
                await _context.SaveChangesAsync();
                return chatToRemove;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred deleting chat.");
                return null;
            }

        }
        public async Task<Message?> AddMessageToChatDb(Message message)
        {
            try
            {
                message.CreatedAt = DateTime.Now;
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred adding message to chat.");
                return null;
            }
        }
        public async Task<List<Message>> GetMessagesByChatIdDb(int chatId)
        {
            try
            {
                List<Message> messages = await _context.Messages.Where(m => m.ChatID == chatId).ToListAsync();
                return messages;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting messages by chat id.");
                return new List<Message>();
            }
        }
        public async Task<int> MarkAllMessagesAsReadDb(int chatId, int currentDogId)
        {
            try
            {
                var messagesToUpdate = await _context.Messages
                    .Where(m => m.ChatID == chatId &&
                                m.SenderDogId != currentDogId &&
                                !m.IsRead)
                    .ToListAsync();

                if (!messagesToUpdate.Any())
                {
                    return 0; 
                }

                foreach (var message in messagesToUpdate)
                {
                    message.IsRead = true;
                }

                var updatedCount = await _context.SaveChangesAsync();
                return updatedCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while marking messages as read.");
                return -1;
            }
        }
    }
}
