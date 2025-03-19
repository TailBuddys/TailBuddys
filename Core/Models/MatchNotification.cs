using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class MatchNotification
    {
        public int Id { get; set; }
        public int DogId { get; set; }
        [JsonIgnore]
        public Dog? Dog { get; set; }
        public int MatchId { get; set; }
    }
}
