using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Park
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required, StringLength(500, MinimumLength = 2)]
        public string? Description { get; set; }
        [Required]
        public string? Address { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
        [JsonIgnore]
        public ICollection<Dog> DogLikes { get; set; } = new List<Dog>();
        [JsonIgnore]
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
