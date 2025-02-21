using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string ChatID { get; set; }
        [JsonIgnore]
        public Chat Chat { get; set; }
        // לשנות את איז סנדר להיות סטרינג סנדר איי די בלי לקשר קשר יחיד רבים
        // הסנדר איי די יהיה מספר המזהה של הכלב השולח
        public bool IsSender { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
