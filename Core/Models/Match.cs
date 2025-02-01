using System.ComponentModel.DataAnnotations.Schema;

namespace TailBuddys.Core.Models
{
    public class Match
    {
        public int Id { get; set; }
        [ForeignKey(nameof(FromDog))]
        public string FromDogId { get; set; }
        public Dog FromDog { get; set; }
        [ForeignKey(nameof(ToDog))]
        public string ToDogId { get; set; }
        public Dog ToDog { get; set; }
        public bool IsLike { get; set; }
        public bool IsMatch { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
