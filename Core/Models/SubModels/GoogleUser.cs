using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models.SubModels
{
    public class GoogleUser
    {
        [JsonPropertyName("sub")]
        public string Sub { get; set; } = string.Empty;
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("given_name")]
        public string GivenName { get; set; } = string.Empty;

    [JsonPropertyName("name")]
        public string FamilyName { get; set; } = string.Empty;

    [JsonPropertyName("aud")]
        public string Audience { get; set; } = string.Empty;
    }
}
