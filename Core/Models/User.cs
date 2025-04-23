using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required, StringLength(20, MinimumLength = 2)]
        public string? FirstName { get; set; }
        [Required, StringLength(20, MinimumLength = 2)]
        public string? LastName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; } = "";
        [Phone]
        public string? Phone { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime? BirthDate { get; set; }
        public Gender? Gender { get; set; }
        [JsonIgnore]
        public bool IsAdmin { get; set; } = false;
        [JsonIgnore]
        public ICollection<Dog> Dogs { get; set; } = new List<Dog>();
        public string? GoogleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public enum Gender
    {
        Male,
        Female,
        Other
    }

}
