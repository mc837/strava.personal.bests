namespace strava.personal.bests.api.Models
{
    public class StravaApiSettings
    {
        public string BaseUrl { get; set; }
        public string ApiV3 { get; set; }
        public string StravaApiV3BaseUrl => $"{BaseUrl}{ApiV3}";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}