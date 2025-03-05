using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Image
    {
        public int Id { get; set; }
        public int EntityId { get; set; }
        public EntityType EntityType { get; set; }
        [Required, Url]
        public string? Url { get; set; }
        [Required]
        public int Order { get; set; }
        [JsonIgnore]
        public Dog? Dog { get; set; }
        [JsonIgnore]
        public Park? Park { get; set; }
    }

    public enum EntityType
    {
        Dog,
        Park
    }
}
