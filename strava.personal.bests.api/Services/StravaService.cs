using Microsoft.Extensions.Options;
using strava.personal.bests.api.Models;
using strava.personal.bests.api.Services.Interfaces;
using System.Net.Http;
using System.Threading.Tasks;

namespace strava.personal.bests.api.Services
{
    public class StravaService : IStravaService
    {
        private readonly StravaApiSettings _stravaApiSettings;
        private readonly IHttpClientFactory _clientFactory;

        public StravaService(IOptions<StravaApiSettings> stravaApiSettings, IHttpClientFactory clientFactory)
        {
            _stravaApiSettings = stravaApiSettings.Value;
            _clientFactory = clientFactory;
        }

        public async Task<HttpResponseMessage> GetToken<T>(T requestBody)
        {
            var request = new RequestBuilder()
                .New(HttpMethod.Post, $"{_stravaApiSettings.BaseUrl}/oauth/token")
                .WithContent(requestBody)
                .Create();

            return await Send(request);
        }

        public async Task<HttpResponseMessage> GetAthlete(string accessToken)
        {
            var request = new RequestBuilder()
                .New(HttpMethod.Get, $"{_stravaApiSettings.StravaApiV3BaseUrl}/athlete")
                .WithAuthHeader(accessToken)
                .Create();

            return await Send(request);
        }

        public async Task<HttpResponseMessage> Deauthorize(string accessToken)
        {
            var request = new RequestBuilder()
                .New(HttpMethod.Post, $"{_stravaApiSettings.BaseUrl}/oauth/deauthorize")
                .WithAuthHeader(accessToken)
                .Create();

            return await Send(request);
        }

        private async Task<HttpResponseMessage> Send(HttpRequestMessage request)
        {
            var client = _clientFactory.CreateClient();
            return await client.SendAsync(request);
        }
    }
}
