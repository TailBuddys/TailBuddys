namespace TailBuddys.Core.Models
{
    public class Messege
    {
        public string Id { get; set; }
        public string ChatID { get; set; }
        //public Chat Chat { get; set; }
        public bool IsFromDog { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
