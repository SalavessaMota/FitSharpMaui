using System.Text.Json.Serialization;

namespace FitSharpMaui.Models
{
    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("expiration")]
        public string Expiration { get; set; }

        [JsonPropertyName("userId")]
        public string UserId { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }
    }
}