using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Image
    {
        public string Id { get; set; }
        public string EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public string Url { get; set; }
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
