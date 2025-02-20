using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Match
    {
        public int Id { get; set; }
        [ForeignKey(nameof(FromDog))]
        public string FromDogId { get; set; }
        [JsonIgnore]
        public Dog? FromDog { get; set; }
        [ForeignKey(nameof(ToDog))]
        public string ToDogId { get; set; }
        [JsonIgnore]
        public Dog? ToDog { get; set; }
        public bool IsLike { get; set; }
        public bool IsMatch { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
