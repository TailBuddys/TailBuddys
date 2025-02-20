using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Chat
    {
        public string Id { get; set; }
        [ForeignKey(nameof(FromDog))]
        public string FromDogId { get; set; }
        [JsonIgnore]
        public Dog FromDog { get; set; }
        [ForeignKey(nameof(ToDog))]
        public string ToDogId { get; set; }
        [JsonIgnore]
        public Dog ToDog { get; set; }
        //public string LastMessegeId { get; set; }
        //public Messege LastMessege { get; set; }
        public bool IsActive { get; set; }
        [JsonIgnore]
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
