using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public int ChatID { get; set; }
        [JsonIgnore]
        public Chat? Chat { get; set; }
        public string? SenderDogId { get; set; }
        [Required, StringLength(500, MinimumLength = 1)]
        public string? Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
