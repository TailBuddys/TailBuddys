using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string ChatID { get; set; }
        [JsonIgnore]
        public Chat Chat { get; set; }
        public bool IsFromDog { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
