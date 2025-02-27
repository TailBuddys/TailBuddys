using System.ComponentModel.DataAnnotations;

namespace TailBuddys.Core.Models.SubModels
{
    public class LoginModel
    {
        [EmailAddress(ErrorMessage = "Email must be valid structure.")]
        public string? Email { get; set; }
        [RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_]).{5,}$", ErrorMessage = "Password must be at least 5 characters long, contain at least one uppercase letter, and one special character.")]
        public string? Password { get; set; }
        public string? GoogleId { get; set; }
    }
}
