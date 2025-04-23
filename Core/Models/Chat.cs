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
        public bool SenderDogArchive { get; set; }
        public bool ReceiverDogArchive { get; set; } 

        [JsonIgnore]
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
