namespace TailBuddys.Core.Models.DTO
{
    public class FullChatDTO
    {
        public int Id { get; set; }
        public UserDogDTO SenderDog { get; set; } = new UserDogDTO();
        public UserDogDTO ReceiverDog { get; set; } = new UserDogDTO();
        public List<Message> Messages { get; set; } = new List<Message> { };
    }
}
