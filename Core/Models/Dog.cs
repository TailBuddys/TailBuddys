using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace TailBuddys.Core.Models
{
    public class Dog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DogType Type { get; set; }
        public DogSize Size { get; set; }
        public bool Geneder { get; set; }
        public DateTime Birthdate { get; set; }
        public ICollection<Image> Images { get; set; }
        public string Address { get; set; }
        public decimal Lon { get; set; }
        public decimal Lat { get; set; }
        public bool Vaccinated { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<Match> Matches { get; set; } = new List<Match>();
        public ICollection<Park> FavParks { get; set; } = new List<Park>();
        public ICollection<Chat> Chats { get; set; } = new List<Chat>();
        public string moshe { get; set; }
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
