using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class ChatNotification
    {
        public int Id { get; set; }
        public int DogId { get; set; }  
        [JsonIgnore]
        public Dog? Dog { get; set; }
        public int ChatId { get; set; } 
        public int UnreadCount { get; set; } 
    }
}
