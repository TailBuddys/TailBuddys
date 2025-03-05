using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Image
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Dog))]
        public int? DogId { get; set; }
        [ForeignKey(nameof(Park))]
        public int? ParkId { get; set; }
        [Required, Url]
        public string? Url { get; set; }
        [Required]
        public int Order { get; set; }
        [JsonIgnore]
        public Dog? Dog { get; set; }
        [JsonIgnore]
        public Park? Park { get; set; }
    }


}
