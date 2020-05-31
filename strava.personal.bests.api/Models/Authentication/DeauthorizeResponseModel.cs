namespace strava.personal.bests.api.Models.Authentication
{
    public class DeauthorizeResponseModel
    {
        public DeauthorizeResponseModel(bool isSuccessStatusCode)
        {
            Deauthorized = isSuccessStatusCode;
        }

        public bool Deauthorized { get; }
    }
}