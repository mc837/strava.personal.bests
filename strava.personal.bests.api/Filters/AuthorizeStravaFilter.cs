using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using strava.personal.bests.api.Models;
using strava.personal.bests.api.Models.Authentication;
using strava.personal.bests.api.Services;
using System.Threading.Tasks;

namespace strava.personal.bests.api.Filters
{
    public class AuthorizeStravaFilter : IAsyncAuthorizationFilter
    {
        private readonly IOptions<StravaPersonalBestsSettings> _settings;
        private readonly ICrypto _crypto;

        public AuthorizeStravaFilter(IOptions<StravaPersonalBestsSettings> settings, ICrypto crypto)
        {
            _settings = settings;
            _crypto = crypto;
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
            context.HttpContext.Items["access_token"] = stravaTokenModel.AccessToken;
            //context.HttpContext.Response.Cookies.Append("spb", "diditwork");
        }
    }

    public class AuthorizeStravaAttribute : TypeFilterAttribute
    {
        public AuthorizeStravaAttribute() : base(typeof(AuthorizeStravaFilter)) { }
    }
}