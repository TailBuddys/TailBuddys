using TailBuddys.Application.Interfaces;

namespace TailBuddys.Application.Services
{
    public class NotificationService : INotificationService
    {
        // סרוויס נוטיפקציה שנועד לעדכן את הדאטה בייס
        //private readonly TailBuddysContext _context;
        //private readonly ILogger<NotificationService> _logger;

        //public NotificationService(AppDbContext context, ILogger<NotificationService> logger)
        //{
        //    _context = context;
        //    _logger = logger;
        //}

        //public async Task<int> AddNotification(string dogId)
        //{
        //    try
        //    {
        //        var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.DogId == dogId);
        //        if (notification == null)
        //        {
        //            notification = new Notification
        //            {
        //                DogId = dogId,
        //                UnreadMessages = 1,
        //                LastUpdated = DateTime.UtcNow
        //            };
        //            _context.Notifications.Add(notification);
        //        }
        //        else
        //        {
        //            notification.UnreadMessages++;
        //            notification.LastUpdated = DateTime.UtcNow;
        //        }
        //        await _context.SaveChangesAsync();
        //        return notification.UnreadMessages;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error adding notification for DogId {dogId}: {ex.Message}");
        //        throw;
        //    }
        //}

        //public async Task MarkAsRead(string dogId)
        //{
        //    try
        //    {
        //        var notification = await _context.Notifications.FirstOrDefaultAsync(n => n.DogId == dogId);
        //        if (notification != null)
        //        {
        //            notification.UnreadMessages = 0;
        //            notification.LastUpdated = DateTime.UtcNow;
        //            await _context.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error marking notification as read for DogId {dogId}: {ex.Message}");
        //        throw;
        //    }
        //}
    }
}
