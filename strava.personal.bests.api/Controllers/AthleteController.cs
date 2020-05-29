using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using strava.personal.bests.api.Filters;
using strava.personal.bests.api.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace strava.personal.bests.api.Controllers
{
    [ApiController]
    public class AthleteController : CustomApiController
    {
        private readonly IHttpClientFactory _clientFactory;

        public AthleteController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [AuthorizeStrava]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAthlete()
        {
            HttpContext.Items.TryGetValue("access_token", out var accessToken);
            var client = _clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://www.strava.com/api/v3/athlete");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.ToString());

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