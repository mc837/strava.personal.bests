namespace strava.personal.bests.api.Models.Authentication
{
    public class AuthenticationRefreshResponseModel
    {
        public AuthenticationRefreshResponseModel() { }

        public AuthenticationRefreshResponseModel(bool authenticated, string encryptedAuthCookie, string accessToken)
        {
            Authenticated = authenticated;
            EncryptedAuthCookie = encryptedAuthCookie;
            AccessToken = accessToken;
        }

        public bool Authenticated { get; }
        public string EncryptedAuthCookie { get; }
        public string AccessToken { get; }
    }
}