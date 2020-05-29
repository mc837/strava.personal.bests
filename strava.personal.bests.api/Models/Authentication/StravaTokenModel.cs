using Newtonsoft.Json;

namespace strava.personal.bests.api.Models.Authentication
{
    public class StravaTokenModel
    {
        [JsonProperty("token_type")] public string TokenType { get; set; }
        [JsonProperty("expires_at")] public string ExpiresAt { get; set; }
        [JsonProperty("expires_in")] public string ExpiresIn { get; set; }
        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
        [JsonProperty("access_token")] public string AccessToken { get; set; }
        [JsonProperty("athlete")] public Athlete Athlete { get; set; }
    }
}