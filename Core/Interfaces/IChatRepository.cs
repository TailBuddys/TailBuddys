﻿using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface IChatRepository
    {
        public Task<Chat?> CreateChatDb(Chat chat);
        public Task<List<Chat>> GetAllDogChatsDb(int dogId);
        public Task<Chat?> GetChatByIdDb(int chatId);
        public Task<Chat?> UpdateChatDb(int chatId, bool isArchive, int clientDogId);
        public Task<Chat?> DeleteChatDb(int chatId);
        public Task<Message?> AddMessageToChatDb(Message message);
        public Task<List<Message>> GetMessagesByChatIdDb(int chatId);
        public Task<int> MarkAllMessagesAsReadDb(int chatId, int currentDogId);
    }
}
