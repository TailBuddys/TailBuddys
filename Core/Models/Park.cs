using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Park
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Address { get; set; }
        public decimal Lon { get; set; }
        public decimal Lat { get; set; }
        [JsonIgnore]
        public ICollection<Dog> DogLikes { get; set; } = new List<Dog>();
        [JsonIgnore]
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
