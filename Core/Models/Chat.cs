using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class Chat
    {
        public int Id { get; set; }
        [ForeignKey(nameof(SenderDog))]
        public int SenderDogId { get; set; }
        [JsonIgnore]
        public Dog? SenderDog { get; set; }
        [ForeignKey(nameof(ReceiverDog))]
        public int ReceiverDogId { get; set; }
        [JsonIgnore]
        public Dog? ReceiverDog { get; set; }
        //public string LastMessegeId { get; set; }
        //public Messege LastMessege { get; set; }
        //public bool IsActive { get; set; }
        [JsonIgnore]
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
