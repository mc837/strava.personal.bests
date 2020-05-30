namespace strava.personal.bests.api.Models.Authentication
{
    public class AuthenticationResponseModel
    {
        public AuthenticationResponseModel() { }

        public AuthenticationResponseModel(bool authenticated, string encryptedAuthCookie, Athlete athlete)
        {
            Authenticated = authenticated;
            EncryptedAuthCookie = encryptedAuthCookie;
            Athlete = athlete;
        }

        public bool Authenticated { get; }
        public string EncryptedAuthCookie { get; }
        public Athlete Athlete { get; }
    }
}