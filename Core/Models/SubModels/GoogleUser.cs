using System.Text.Json.Serialization;

namespace TailBuddys.Core.Models.SubModels
{
    public class GoogleUser
    {
        [JsonPropertyName("sub")]
        public string Sub { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("given_name")]
        public string GivenName { get; set; }

        [JsonPropertyName("name")]
        public string FamilyName { get; set; }

        [JsonPropertyName("aud")]
        public string Audience { get; set; }
    }
}
