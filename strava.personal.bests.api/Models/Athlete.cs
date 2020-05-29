using Newtonsoft.Json;

namespace strava.personal.bests.api.Models
{
    public class Athlete
    {
        [JsonProperty("id")] public long Id { get; set; }
        [JsonProperty("username")] public string Username { get; set; }
        [JsonProperty("firstname")] public string FirstName { get; set; }
        [JsonProperty("lastname")] public string LastName { get; set; }
        [JsonProperty("city")] public string City { get; set; }
        [JsonProperty("state")] public string Country { get; set; }
        [JsonProperty("sex")] public string Sex { get; set; }
        [JsonProperty("premium")] public string Premium { get; set; }
        [JsonProperty("summit")] public string Summit { get; set; }
        [JsonProperty("created_at")] public string CreatedAt { get; set; }
        [JsonProperty("updated_at")] public string updatedAt { get; set; }
        [JsonProperty("badge_type")] public string BadgeType { get; set; }
        [JsonProperty("profile_medium")] public string ProfileMedium { get; set; }
        [JsonProperty("profile")] public string Profile { get; set; }

    }
}