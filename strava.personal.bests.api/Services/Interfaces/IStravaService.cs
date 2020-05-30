using strava.personal.bests.api.Models.Authentication;
using System.Net.Http;
using System.Threading.Tasks;

namespace strava.personal.bests.api.Services.Interfaces
{
    public interface IStravaService
    {
        Task<HttpResponseMessage> GetToken(AuthenticationRequestModel request);
        Task<HttpResponseMessage> GetAthlete(string accessToken);
    }
}
