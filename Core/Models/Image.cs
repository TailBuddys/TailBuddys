namespace TailBuddys.Core.Models
{
    public class Image
    {
        public string Id { get; set; }
        public string EntityId { get; set; }
        public EntityType EntityType { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
        public Dog Dog { get; set; }
        public Park Park { get; set; }
    }

    public enum EntityType
    {
        Dog,
        Park
    }
}
