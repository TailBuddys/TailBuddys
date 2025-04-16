namespace TailBuddys.Core.Models.DTO
{
    public class DogsFilterDTO
    {
        public int? Distance { get; set; }
        public List<DogType?>? Type { get; set; }
        public List<DogSize?>? Size { get; set; }
        public List<bool?> Gender { get; set; } = new List<bool?>();
        public List<bool?> Vaccinated { get; set; } = new List<bool?>();
    }
}
