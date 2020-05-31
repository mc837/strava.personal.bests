using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using strava.personal.bests.api.Models;
using strava.personal.bests.api.Models.Authentication;
using strava.personal.bests.api.Services;
using strava.personal.bests.api.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace strava.personal.bests.api.Filters
{
    public class AuthorizeStravaFilter : IAsyncAuthorizationFilter
    {
        private readonly IOptions<StravaPersonalBestsSettings> _settings;
        private readonly ICrypto _crypto;
        private readonly IStravaAuthService _stravaAuthService;

        public AuthorizeStravaFilter(IOptions<StravaPersonalBestsSettings> settings, ICrypto crypto, IStravaAuthService stravaAuthService)
        {
            _settings = settings;
            _crypto = crypto;
            _stravaAuthService = stravaAuthService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            context.HttpContext.Request.Cookies.TryGetValue("spb", out var spbAuthCookie);
            if (spbAuthCookie == null)
            {
                context.Result = new UnauthorizedResult();
            }

            var decryptedCookie = _crypto.Decrypt(_settings.Value.CryptoSecret, spbAuthCookie);
            var stravaTokenModel = JsonConvert.DeserializeObject<StravaTokenModel>(decryptedCookie);

            long.TryParse(stravaTokenModel.ExpiresAt, out var tokenExpiry);

            if (tokenExpiry - DateTimeOffset.UtcNow.ToUnixTimeSeconds() > 20000)
            {
                context.HttpContext.Items["access_token"] = stravaTokenModel.AccessToken;
                return;
            }

            var refreshedAuthenticationResult = await _stravaAuthService.GetRefreshedToken(stravaTokenModel.RefreshToken);

            if (refreshedAuthenticationResult.Authenticated == false) context.Result = new UnauthorizedResult(); //TODO: logging/ handle better?

            context.HttpContext.Response.Cookies.Append("spb", refreshedAuthenticationResult.EncryptedAuthCookie);
            context.HttpContext.Items["access_token"] = refreshedAuthenticationResult.AccessToken;
        }
    }



    public class AuthorizeStravaAttribute : TypeFilterAttribute
    {
        public AuthorizeStravaAttribute() : base(typeof(AuthorizeStravaFilter)) { }
    }
}