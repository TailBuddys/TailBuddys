using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Match
    {
        public int Id { get; set; }
        [ForeignKey(nameof(SenderDog))]
        public string SenderDogId { get; set; }
        [JsonIgnore]
        public Dog? SenderDog { get; set; }
        [ForeignKey(nameof(ReciverDog))]
        public string ReciverDogId { get; set; }
        [JsonIgnore]
        public Dog? ReciverDog { get; set; }
        public bool IsLike { get; set; }
        public bool IsMatch { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
