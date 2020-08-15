using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using strava.personal.bests.api.Filters;
using strava.personal.bests.api.Models;
using strava.personal.bests.api.Services.Interfaces;
using System.Threading.Tasks;

namespace strava.personal.bests.api.Controllers
{
    [ApiController]
    public class AthleteController : CustomApiController
    {
        private readonly IStravaService _stravaService;

        public AthleteController(IStravaService stravaService)
        {
            _stravaService = stravaService;
        }

        [AuthorizeStrava]
        [EnableCors("Policy1")]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAthlete()
        {
            HttpContext.Items.TryGetValue("access_token", out var accessToken); // TODO: put in another filter then link to ctrl prop?
            var response = await _stravaService.GetAthlete(accessToken.ToString());
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return new BadRequestResult();

            var athlete = JsonConvert.DeserializeObject<Athlete>(content);

            return new OkObjectResult(athlete);
        }
    }
}