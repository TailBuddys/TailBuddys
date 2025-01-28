namespace TailBuddys.Core.Models
{
    public class Park
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Lon { get; set; } // מה זה?
        public decimal Lat { get; set; } // מה זה?
        public ICollection<Dog> DogLikes { get; set; } = new List<Dog>();
        public ICollection<Image> Images { get; set; } = new List<Image>();
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
