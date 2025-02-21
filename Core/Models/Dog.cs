using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Dog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; } // לבדוק למה צריך יוזר בתוך המודול של כלב
        public string Name { get; set; }
        public string Description { get; set; }
        public DogType Type { get; set; }
        public DogSize Size { get; set; }
        public bool Geneder { get; set; }
        public DateTime Birthdate { get; set; }
        [JsonIgnore]
        public ICollection<Image>? Images { get; set; }
        public string Address { get; set; }
        public decimal Lon { get; set; } // מה זה?
        public decimal Lat { get; set; } // מה זה?
        public bool Vaccinated { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [JsonIgnore]
        public ICollection<Match> MatchesAsSender { get; set; } = new List<Match>();
        [JsonIgnore]
        public ICollection<Match> MatchesAsReciver { get; set; } = new List<Match>();
        [JsonIgnore]
        public ICollection<Park> FavParks { get; set; } = new List<Park>();
        [JsonIgnore]
        public ICollection<Chat> ChatsAsSender { get; set; } = new List<Chat>();
        [JsonIgnore]
        public ICollection<Chat> ChatsAsReciver { get; set; } = new List<Chat>();
        [JsonIgnore]
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }

    public enum DogType
    {
        Mixed,
        labrador,
        other
    }

    public enum DogSize
    {
        Small,
        Medium,
        Large
    }
}
