namespace TailBuddys.Core.Models.DTO
{
    public class DogsFilterDTO
    {
        public int? distance { get; set; }
        public List<DogType?>? Type { get; set; }
        public List<DogSize?>? Size { get; set; }
        public bool? Geneder { get; set; }
        public bool? Vaccinated { get; set; }
    }
}
