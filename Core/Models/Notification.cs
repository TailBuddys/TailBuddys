using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public int DogId { get; set; }
        [JsonIgnore]
        public Dog? Dog { get; set; }
        public int UnreadMessages { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
