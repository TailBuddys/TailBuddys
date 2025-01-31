namespace TailBuddys.Core.Models
{
    public class Match
    {
        public int Id { get; set; }
        public string FromDogId { get; set; }
        public Dog FromDog { get; set; }
        public string ToDogId { get; set; }
        public bool IsLike { get; set; }
        public bool IsMatch { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
