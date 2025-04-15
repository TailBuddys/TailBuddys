using TailBuddys.Core.Models.DTO;

namespace TailBuddys.Core.DTO
{
    public class ParkDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
        public double? Distance { get; set; }
        public List<UserDogDTO> DogLikes { get; set; } = new List<UserDogDTO>();
        public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
    }
}
