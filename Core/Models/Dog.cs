namespace TailBuddys.Core.Models
{
    public class Dog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public User User { get; set; } // לבדוק למה צריך יוזר בתוך המודול של כלב
        public string Name { get; set; }
        public string Description { get; set; }
        public DogType Type { get; set; }
        public DogSize Size { get; set; }
        public bool Geneder { get; set; }
        public DateTime Birthdate { get; set; }
        public ICollection<Image> Images { get; set; }
        public string Address { get; set; }
        public decimal Lon { get; set; } // מה זה?
        public decimal Lat { get; set; } // מה זה?
        public bool Vaccinated { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Match> Matches { get; set; } = new List<Match>();
        public ICollection<Park> FavParks { get; set; } = new List<Park>();
        public ICollection<Chat> Chats { get; set; } = new List<Chat>();
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
