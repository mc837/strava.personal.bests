using strava.personal.bests.api.Models.Authentication;
using System.Threading.Tasks;

namespace strava.personal.bests.api.Services.Interfaces
{
    public interface IStravaAuthService
    {
        Task<AuthenticationResponseModel> GetToken(string code);
        AuthenticationResponseModel GetRefreshToken(string code);
    }
}
