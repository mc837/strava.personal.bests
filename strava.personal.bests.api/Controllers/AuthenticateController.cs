using Microsoft.AspNetCore.Mvc;
using strava.personal.bests.api.Models.Authentication;
using strava.personal.bests.api.Services.Interfaces;
using System.Threading.Tasks;

namespace strava.personal.bests.api.Controllers
{
    [ApiController]
    public class AuthenticateController : CustomApiController
    {
        private readonly IStravaAuthService _stravaAuthService;

        public AuthenticateController(
            IStravaAuthService stravaAuthService)
        {
            _stravaAuthService = stravaAuthService;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> GetToken([FromBody] GetToken getTokenModel)
        {
            var authenticationResult = await _stravaAuthService.GetToken(getTokenModel.Code);

            if (authenticationResult.Authenticated == false) return BadRequest(); //TODO: logging/ handle better?

            Response.Cookies.Append(
                "spb", authenticationResult.EncryptedAuthCookie);

            return new OkObjectResult(authenticationResult.Athlete);
        }
    }
}