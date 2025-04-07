using TailBuddys.Core.Models;

namespace TailBuddys.Core.DTO
{
    public class DogDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DogType? Type { get; set; }
        public DogSize? Size { get; set; }
        public bool? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public List<string> Images { get; set; } = new List<string>();
        public string? Address { get; set; }
        public double Lon { get; set; }
        public double Lat { get; set; }
        public double? Distance { get; set; }
        public bool? Vaccinated { get; set; }
        public string? RefreshToken { get; set; }

    }
}
