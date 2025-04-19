using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models.DTO
{
    public class UserDogDTO
    {
        public int Id { get; set; }
        [JsonIgnore]
        public bool? IsBot { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}
