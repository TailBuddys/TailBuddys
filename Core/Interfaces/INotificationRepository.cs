using TailBuddys.Core.Models;

namespace TailBuddys.Core.Interfaces
{
    public interface INotificationRepository
    {
        public Task<Notification?> CreateNotification(string id);
        public Task<List<Notification?>> GetAllDogNotification(string id);
        public Task<Notification?> GetNotificationById(string id);
        public Task<Notification?> UpdateNotification(string id);
        public Task<Notification?> DeleteNotification(string id);
    }
}
