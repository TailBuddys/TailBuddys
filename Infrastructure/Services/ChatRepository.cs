using Microsoft.EntityFrameworkCore;
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
        public async Task<Chat?> CreateChatDb(Chat chat)
        {
            try
            {
                _context.Chats.Add(chat);
                //_context.Messeges.Add(chat.Messages.FirstOrDefault());  ????
                await _context.SaveChangesAsync();
                return chat;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
                Console.WriteLine(e);
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
                Console.WriteLine(ex);
                return null;
            }
        }
        public async Task<Chat?> UpdateChatDb(int chatId, Chat chat)
        {
            try
            {
                Chat? chatToUpdate = await _context.Chats.FirstOrDefaultAsync(c => c.Id == chatId);
                if (chatToUpdate == null)
                {
                    return null;
                }

                // ככל הנראה יבוטל
                //chatToUpdate.IsActive = chat.IsActive;

                _context.Chats.Update(chatToUpdate);
                await _context.SaveChangesAsync();
                return chatToUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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
                Console.WriteLine(ex);
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
                Console.WriteLine(ex);
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
                Console.WriteLine(ex);
                return new List<Message>();
            }
        }
        public async Task<Message?> MarkMessageAsReadDb(int messageId)
        {
            try
            {
                Message? messageToUpdate = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
                if (messageToUpdate == null)
                {
                    return null;
                }

                messageToUpdate.IsRead = true;

                _context.Messages.Update(messageToUpdate);
                await _context.SaveChangesAsync();
                return messageToUpdate;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

        }
    }
}
