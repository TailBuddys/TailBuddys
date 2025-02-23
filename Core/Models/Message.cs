using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int ChatID { get; set; }
        [JsonIgnore]
        public Chat? Chat { get; set; }
        public string? SenderDogId { get; set; }
        public string? Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
