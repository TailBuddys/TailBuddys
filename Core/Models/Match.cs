using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Match
    {
        public int Id { get; set; }
        [ForeignKey(nameof(SenderDog))]
        public int SenderDogId { get; set; }
        [JsonIgnore]
        public Dog? SenderDog { get; set; }
        [ForeignKey(nameof(ReceiverDog))]
        public int ReceiverDogId { get; set; }
        [JsonIgnore]
        public Dog? ReceiverDog { get; set; }
        [Required]
        public bool IsLike { get; set; }
        public bool IsMatch { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [JsonIgnore]
        public ICollection<MatchNotification> MatchNotification { get; set; } = new List<MatchNotification>();
    }
}
