using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class MatchNotification
    {
        public int Id { get; set; }
        [ForeignKey(nameof(Dog))]
        public int DogId { get; set; }
        [JsonIgnore]
        public Dog? Dog { get; set; }
        [ForeignKey(nameof(Match))]
        public int MatchId { get; set; }
        [JsonIgnore]
        public Match? Match { get; set; }
    }
}
