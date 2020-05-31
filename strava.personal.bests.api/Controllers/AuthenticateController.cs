using Microsoft.AspNetCore.Mvc;
using strava.personal.bests.api.Filters;
using strava.personal.bests.api.Models.Authentication;
using strava.personal.bests.api.Services.Interfaces;
using System;
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
                "spb", authenticationResult.EncryptedAuthCookie, new Microsoft.AspNetCore.Http.CookieOptions
                {
                    Expires = DateTime.UtcNow.AddDays(10)
                });

            return new OkObjectResult(authenticationResult.Athlete);
        }

        [AuthorizeStrava]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Deauthorize()
        {
            HttpContext.Items.TryGetValue("access_token", out var accessToken); // helper function should throw if access_token is null.

            var result = await _stravaAuthService.Deauthorize(accessToken.ToString());

            if (result.Deauthorized == false) return BadRequest(); //TODO: logging/ handle better?

            Response.Cookies.Delete("spb");

            return new OkResult();
        }
    }
}