namespace TailBuddys.Core.Models
{
    public class Chat
    {
        public string Id { get; set; }
        public string FromDogId { get; set; }
        //public Dog FromDog { get; set; }
        public string ToDogId { get; set; }
        //public Dog ToDog { get; set; }
        //public string LastMessegeId { get; set; }
        //public Messege LastMessege { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Messege> Messeges { get; set; } = new List<Messege>();
        public string Noy {  get; set; }
    }
}
