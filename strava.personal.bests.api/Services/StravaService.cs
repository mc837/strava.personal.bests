using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using strava.personal.bests.api.Models;
using strava.personal.bests.api.Models.Authentication;
using strava.personal.bests.api.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        public async Task<HttpResponseMessage> GetToken(AuthenticationRequestModel requestBody)
        {
            var authRequest = JsonConvert.SerializeObject(requestBody);
            return await Post(authRequest, $"{_stravaApiSettings.BaseUrl}/oauth/token");
        }

        public async Task<HttpResponseMessage> GetAthlete(string accessToken)
        {
            return await Get(accessToken, $"{_stravaApiSettings.StravaApiV3BaseUrl}/athlete");
        }

        private async Task<HttpResponseMessage> Post(string content, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };

            return await Send(request);
        }
        private async Task<HttpResponseMessage> Get(string accessToken, string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            return await Send(request);
        }

        private async Task<HttpResponseMessage> Send(HttpRequestMessage request)
        {
            var client = _clientFactory.CreateClient();
            return await client.SendAsync(request);
        }
    }
}
