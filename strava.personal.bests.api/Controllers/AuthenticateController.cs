using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using strava.personal.bests.api.Services;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace strava.personal.bests.api.Controllers
{
    [ApiController]
    public class AuthenticateController : CustomApiController
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly ICrypto _crypto;

        public AuthenticateController(IHttpClientFactory clientFactory, ICrypto crypto)
        {
            _clientFactory = clientFactory;
            _crypto = crypto;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetToken([FromBody] GetToken getTokenModel)
        {
            var authenticationRequestModel = new AuthenticationRequestModel
            {
                client_id = 1,
                client_secret = "",
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

            if (response.IsSuccessStatusCode)
            {
                var res = new OkObjectResult(result.Athlete);
                var cookieValue = JsonConvert.SerializeObject(result);
                Response.Cookies.Append(
                    "spb", _crypto.Encrypt("test", cookieValue));

                return res;
            }

            return BadRequest();

        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAthlete()
        {
            Request.Cookies.TryGetValue("spb", out string myCookie);
            var decryptedCookie = _crypto.Decrypt("test", myCookie);
            var stravaTokenModel = JsonConvert.DeserializeObject<StravaTokenModel>(decryptedCookie);

            var client = _clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.strava.com/api/v3/athlete");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", stravaTokenModel.AccessToken);

            var response = await client.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Athlete>(content);

            if (response.IsSuccessStatusCode)
            {
                var res = new OkObjectResult(result);
                return res;
            }

            return new BadRequestResult();
        }
    }
}

public class StravaTokenModel
{
    [JsonProperty("token_type")] public string TokenType { get; set; }
    [JsonProperty("expires_at")] public string ExpiresAt { get; set; }
    [JsonProperty("expires_in")] public string ExpiresIn { get; set; }
    [JsonProperty("refresh_token")] public string RefreshToken { get; set; }
    [JsonProperty("access_token")] public string AccessToken { get; set; }
    [JsonProperty("athlete")] public Athlete Athlete { get; set; }
}

public class Athlete
{
    [JsonProperty("id")] public long Id { get; set; }
    [JsonProperty("username")] public string Username { get; set; }
    [JsonProperty("firstname")] public string FirstName { get; set; }
    [JsonProperty("lastname")] public string LastName { get; set; }
    [JsonProperty("city")] public string City { get; set; }
    [JsonProperty("state")] public string Country { get; set; }
    [JsonProperty("sex")] public string Sex { get; set; }
    [JsonProperty("premium")] public string Premium { get; set; }
    [JsonProperty("summit")] public string Summit { get; set; }
    [JsonProperty("created_at")] public string CreatedAt { get; set; }
    [JsonProperty("updated_at")] public string updatedAt { get; set; }
    [JsonProperty("badge_type")] public string BadgeType { get; set; }
    [JsonProperty("profile_medium")] public string ProfileMedium { get; set; }
    [JsonProperty("profile")] public string Profile { get; set; }

}


public class GetToken
{
    public string Code { get; set; }
}

public class AuthenticationRequestModel
{
    public int client_id { get; set; }
    public string client_secret { get; set; }
    public string code { get; set; }
    public string grant_type { get; set; }
}