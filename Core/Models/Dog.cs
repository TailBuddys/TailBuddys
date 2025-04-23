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
        [StringLength(500, MinimumLength = 2)]
        public string? Description { get; set; }
        public DogType? Type { get; set; }
        public DogSize? Size { get; set; }
        public bool? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        [JsonIgnore]
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public string Address { get; set; } = "";
        public double Lon { get; set; }
        public double Lat { get; set; }
        public bool? Vaccinated { get; set; }
        public bool? IsBot { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [JsonIgnore]
        public ICollection<Match> MatchesAsSender { get; set; } = new List<Match>();
        [JsonIgnore]
        public ICollection<Match> MatchesAsReceiver { get; set; } = new List<Match>();
        [JsonIgnore]
        public ICollection<Park> FavParks { get; set; } = new List<Park>();
        [JsonIgnore]
        public ICollection<Chat> ChatsAsSender { get; set; } = new List<Chat>();
        [JsonIgnore]
        public ICollection<Chat> ChatsAsReceiver { get; set; } = new List<Chat>();
        [JsonIgnore]
        public ICollection<MatchNotification> MatchNotification { get; set; } = new List<MatchNotification>();
        [JsonIgnore]
        public ICollection<ChatNotification> UnreadChatNotification { get; set; } = new List<ChatNotification>();
    }

    public enum DogType
    {
        Afghan_Hound,
        Akita,
        Alaskan_Malamute,
        American_Bulldog,
        American_Eskimo_Dog,
        American_Foxhound,
        American_Staffordshire_Terrier,
        Anatolian_Shepherd,
        Australian_Cattle_Dog,
        Australian_Shepherd,
        Basenji,
        Basset_Hound,
        Beagle,
        Beauceron,
        Belgian_Malinois,
        Belgian_Sheepdog,
        Belgian_Tervuren,
        Bernese_Mountain_Dog,
        Bichon_Frise,
        Black_and_Tan_Coonhound,
        Bloodhound,
        Border_Collie,
        Border_Terrier,
        Borzoi,
        Boston_Terrier,
        Bouvier_des_Flandres,
        Boxer,
        Briard,
        Brittany_Spaniel,
        Bulldog,
        Bullmastiff,
        Cairn_Terrier,
        Cane_Corso,
        Cavalier_King_Charles_Spaniel,
        Chesapeake_Bay_Retriever,
        Chihuahua,
        Chinese_Crested,
        Chinese_Shar_Pei,
        Chow_Chow,
        Clumber_Spaniel,
        Cocker_Spaniel,
        Collie,
        Curly_Coated_Retriever,
        Dachshund,
        Dalmatian,
        Dandie_Dinmont_Terrier,
        Doberman_Pinscher,
        English_Cocker_Spaniel,
        English_Foxhound,
        English_Setter,
        English_Springer_Spaniel,
        English_Toy_Spaniel,
        Flat_Coated_Retriever,
        French_Bulldog,
        German_Pinscher,
        German_Shepherd,
        German_Shorthaired_Pointer,
        German_Wirehaired_Pointer,
        Giant_Schnauzer,
        Golden_Retriever,
        Gordon_Setter,
        Great_Dane,
        Great_Pyrenees,
        Greyhound,
        Havanese,
        Irish_Setter,
        Irish_Terrier,
        Irish_Water_Spaniel,
        Irish_Wolfhound,
        Italian_Greyhound,
        Jack_Russell_Terrier,
        Japanese_Chin,
        Keeshond,
        Kerry_Blue_Terrier,
        Komondor,
        Kuvasz,
        Labrador_Retriever,
        Lakeland_Terrier,
        Leonberger,
        Lhasa_Apso,
        Maltese,
        Manchester_Terrier,
        Mastiff,
        Miniature_Bull_Terrier,
        Miniature_Pinscher,
        Miniature_Schnauzer,
        Mixed,
        Neapolitan_Mastiff,
        Newfoundland,
        Norfolk_Terrier,
        Norwegian_Elkhound,
        Norwich_Terrier,
        Old_English_Sheepdog,
        Otterhound,
        Papillon,
        Pekingese,
        Pembroke_Welsh_Corgi,
        Petit_Basset_Griffon_Vendeen,
        Pharaoh_Hound,
        Pointer,
        Polish_Lowland_Sheepdog,
        Pomeranian,
        Poodle,
        Portuguese_Water_Dog,
        Pug,
        Puli,
        Rhodesian_Ridgeback,
        Rottweiler,
        Saint_Bernard,
        Saluki,
        Samoyed,
        Schipperke,
        Scottish_Terrier,
        Sealyham_Terrier,
        Shetland_Sheepdog,
        Shiba_Inu,
        Shih_Tzu,
        Siberian_Husky,
        Silky_Terrier,
        Skye_Terrier,
        Soft_Coated_Wheaten_Terrier,
        Staffordshire_Bull_Terrier,
        Standard_Schnauzer,
        Sussex_Spaniel,
        Tibetan_Mastiff,
        Tibetan_Spaniel,
        Tibetan_Terrier,
        Toy_Fox_Terrier,
        Toy_Manchester_Terrier,
        Toy_Poodle,
        Vizsla,
        Weimaraner,
        Welsh_Springer_Spaniel,
        Welsh_Terrier,
        West_Highland_White_Terrier,
        Whippet,
        Wirehaired_Fox_Terrier,
        Wirehaired_Pointing_Griffon,
        Xoloitzcuintli,
        Yorkshire_Terrier,
        Other
    }

    public enum DogSize
    {
        Small,
        Medium,
        Large
    }
}
