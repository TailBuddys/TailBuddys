namespace TailBuddys.Core.Models.DTO
{
    public class DogsFilterDTO
    {
        public int? Distance { get; set; }
        public List<DogType?>? Breeds { get; set; }
        public List<DogSize?>? Size { get; set; }
        public List<bool?>? Gender { get; set; }
        public List<bool?>? Vaccinated { get; set; }
    }
}
