namespace TailBuddys.Core.Models.SubModels
{
    public class GoogleUser
    {
        public string Sub { get; set; } = string.Empty; // Google User ID
        public string Email { get; set; } = string.Empty;
        public string GivenName { get; set; } = string.Empty;
        public string FamilyName { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }
}
