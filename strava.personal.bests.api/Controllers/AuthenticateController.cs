using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using strava.personal.bests.api.Models;
using strava.personal.bests.api.Models.Authentication;
using strava.personal.bests.api.Services;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace strava.personal.bests.api.Controllers
{
    [ApiController]
    public class AuthenticateController : CustomApiController
    {
        private readonly IOptions<StravaApiSettings> _stravaApiSettings;
        private readonly IOptions<StravaPersonalBestsSettings> _stravaPersonalBestsSettings;

        private readonly IHttpClientFactory _clientFactory;
        private readonly ICrypto _crypto;

        public AuthenticateController(
            IOptions<StravaApiSettings> stravaApiSettings,
            IOptions<StravaPersonalBestsSettings> stravaPersonalBestsSettings,
            IHttpClientFactory clientFactory, ICrypto crypto)
        {
            _stravaApiSettings = stravaApiSettings;
            _stravaPersonalBestsSettings = stravaPersonalBestsSettings;
            _clientFactory = clientFactory;
            _crypto = crypto;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetToken([FromBody] GetToken getTokenModel)
        {
            var authenticationRequestModel = new AuthenticationRequestModel
            {
                client_id = _stravaApiSettings.Value.ClientId,
                client_secret = _stravaApiSettings.Value.ClientSecret,
                code = getTokenModel.Code,
                grant_type = "authorization_code"
            };

            var json = JsonConvert.SerializeObject(authenticationRequestModel);
            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://www.strava.com/oauth/token")
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StravaTokenModel>(content);

            if (!response.IsSuccessStatusCode) return BadRequest();

            var res = new OkObjectResult(result.Athlete);
            var cookieValue = JsonConvert.SerializeObject(result);
            Response.Cookies.Append(
                "spb", _crypto.Encrypt(_stravaPersonalBestsSettings.Value.CryptoSecret, cookieValue));

            return res;
        }
    }
}