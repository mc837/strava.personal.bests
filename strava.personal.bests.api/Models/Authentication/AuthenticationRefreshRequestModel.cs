namespace strava.personal.bests.api.Models.Authentication
{
    public class AuthenticationRefreshRequestModel
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string refresh_token { get; set; }
        public string grant_type { get; set; }
    }
}