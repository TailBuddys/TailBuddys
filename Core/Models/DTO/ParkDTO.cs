namespace TailBuddys.Core.DTO
{
    public class ParkDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Address { get; set; }
        public int DogLikes { get; set; }
        public double? Distance { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }
}
