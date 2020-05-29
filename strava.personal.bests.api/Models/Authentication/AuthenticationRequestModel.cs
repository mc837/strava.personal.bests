namespace strava.personal.bests.api.Models.Authentication
{
    public class AuthenticationRequestModel
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string code { get; set; }
        public string grant_type { get; set; }
    }
}