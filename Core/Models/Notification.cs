namespace TailBuddys.Core.Models
{
    public class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string DogId { get; set; }
        public Dog Dog { get; set; }
        public int UnreadMessages { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
