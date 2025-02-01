using System.ComponentModel.DataAnnotations.Schema;

namespace TailBuddys.Core.Models
{
    public class Chat
    {
        public string Id { get; set; }
        [ForeignKey(nameof(FromDog))]
        public string FromDogId { get; set; }
        public Dog FromDog { get; set; }
        [ForeignKey(nameof(ToDog))]
        public string ToDogId { get; set; }
        public Dog ToDog { get; set; }
        //public string LastMessegeId { get; set; }
        //public Messege LastMessege { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
