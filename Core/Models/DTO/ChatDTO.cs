using TailBuddys.Core.Models;
using TailBuddys.Core.Models.DTO;

namespace TailBuddys.Core.DTO
{
    public class ChatDTO
    {
        public int Id { get; set; }
        public UserDogDTO Dog { get; set; } = new UserDogDTO();
        public Message? LastMessage { get; set; }
    }
}
