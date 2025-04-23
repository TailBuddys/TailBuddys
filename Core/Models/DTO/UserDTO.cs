using TailBuddys.Core.Models;

namespace TailBuddys.Core.DTO
{
    public class UserDTO
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender? Gender { get; set; }
    }
}
