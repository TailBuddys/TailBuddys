using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Dog
    {
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        // לבדוק שהנאלל לא בא לנו בהפוכה בהמשך
        public int UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; } // לבדוק למה צריך יוזר בתוך המודול של כלב
        [Required, StringLength(20, MinimumLength = 2)]
        public string? Name { get; set; }
        [Required, StringLength(500, MinimumLength = 2)]
        public string? Description { get; set; }
        public DogType Type { get; set; }
        public DogSize Size { get; set; }
        public bool Geneder { get; set; }
        public DateTime Birthdate { get; set; }
        [JsonIgnore]
        public ICollection<Image> Images { get; set; } = new List<Image>();
        [Required]
        public string? Address { get; set; }
        public decimal Lon { get; set; } // מה זה?
        public decimal Lat { get; set; } // מה זה?
        [Required]
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
        LabradorRetriever,
        GoldenRetriever,
        GermanShepherd,
        Bulldog,
        Poodle,
        Beagle,
        Rottweiler,
        YorkshireTerrier,
        Boxer,
        Dachshund,
        SiberianHusky,
        DobermanPinscher,
        ShihTzu,
        BorderCollie,
        AustralianShepherd,
        CockerSpaniel,
        GreatDane,
        Chihuahua,
        Pug,
        PembrokeWelshCorgi,
        Akita,
        SaintBernard,
        BichonFrise,
        Maltese,
        ShetlandSheepdog,
        BostonTerrier,
        BerneseMountainDog,
        MiniatureSchnauzer,
        WestHighlandWhiteTerrier,
        ShibaInu,
        Havanese,
        CaneCorso,
        Newfoundland,
        EnglishSpringerSpaniel,
        Collie,
        BassetHound,
        Weimaraner,
        RhodesianRidgeback,
        AlaskanMalamute,
        ScottishTerrier,
        Bullmastiff,
        AustralianCattleDog,
        BelgianMalinois,
        AfghanHound,
        AmericanBulldog,
        IrishSetter,
        CavalierKingCharlesSpaniel,
        AmericanStaffordshireTerrier,
        Bloodhound,
        JackRussellTerrier,
        Vizsla,
        BorderTerrier,
        Samoyed,
        GreatPyrenees,
        ChowChow,
        ItalianGreyhound,
        Whippet,
        Keeshond,
        Leonberger,
        NorwegianElkhound,
        Papillon,
        Pointer,
        StandardSchnauzer,
        ToyPoodle,
        IrishWolfhound,
        EnglishFoxhound,
        GiantSchnauzer,
        LhasaApso,
        JapaneseChin,
        TibetanMastiff,
        AmericanEskimoDog,
        PortugueseWaterDog,
        Saluki,
        Borzoi,
        CurlyCoatedRetriever,
        Komondor,
        Otterhound,
        FlatCoatedRetriever,
        Other
    }

    public enum DogSize
    {
        Small,
        Medium,
        Large
    }
}
