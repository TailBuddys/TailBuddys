using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class ChatNotification
    {
        public int Id { get; set; }
        public int DogId { get; set; }  // Receiver dog
        [JsonIgnore]
        public Dog? Dog { get; set; }
        public int ChatId { get; set; } // Chat reference
        public int UnreadCount { get; set; } // Number of unread messages
    }
}
