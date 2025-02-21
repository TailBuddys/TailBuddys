using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string ChatID { get; set; }
        [JsonIgnore]
        public Chat Chat { get; set; }
        // צריך לשנות למזהה של הכלב ששלח את ההודעה  
        public bool IsSender { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
