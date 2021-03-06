﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using strava.personal.bests.api.Models;
using strava.personal.bests.api.Models.Authentication;
using strava.personal.bests.api.Services.Interfaces;
using System.Threading.Tasks;

namespace strava.personal.bests.api.Services
{
    public class StravaAuthService : IStravaAuthService
    {
        private readonly IOptions<StravaApiSettings> _stravaApiSettings;
        private readonly IOptions<StravaPersonalBestsSettings> _stravaPersonalBestsSettings;
        private readonly ICrypto _crypto;
        private readonly IStravaService _stravaService;

        public StravaAuthService(IOptions<StravaApiSettings> stravaApiSettings,
            IOptions<StravaPersonalBestsSettings> stravaPersonalBestsSettings, ICrypto crypto, IStravaService stravaService)
        {
            _stravaApiSettings = stravaApiSettings;
            _stravaPersonalBestsSettings = stravaPersonalBestsSettings;
            _crypto = crypto;
            _stravaService = stravaService;
        }

        public async Task<AuthenticationResponseModel> GetToken(string code)
        {
            var getTokenResponse = await _stravaService.GetToken(GetAuthenticationRequestModel(code));

            var content = await getTokenResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StravaTokenModel>(content);

            if (getTokenResponse.IsSuccessStatusCode == false) return new AuthenticationResponseModel();

            var cookieValue = JsonConvert.SerializeObject(result);
            var encryptedAuthCookie = _crypto.Encrypt(_stravaPersonalBestsSettings.Value.CryptoSecret, cookieValue);
            return new AuthenticationResponseModel(true, encryptedAuthCookie, result.Athlete);
        }

        public async Task<AuthenticationRefreshResponseModel> GetRefreshedToken(string refreshToken)
        {
            var getTokenResponse = await _stravaService.GetToken(GetAuthenticationRefreshRequestModel(refreshToken));

            var content = await getTokenResponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<StravaTokenModel>(content);

            if (getTokenResponse.IsSuccessStatusCode == false) return new AuthenticationRefreshResponseModel();

            var cookieValue = JsonConvert.SerializeObject(result);
            var encryptedAuthCookie = _crypto.Encrypt(_stravaPersonalBestsSettings.Value.CryptoSecret, cookieValue);
            return new AuthenticationRefreshResponseModel(true, encryptedAuthCookie, result.AccessToken);
        }

        public async Task<DeauthorizeResponseModel> Deauthorize(string accessToken)
        {
            var getTokenResponse = await _stravaService.Deauthorize(accessToken);
            return new DeauthorizeResponseModel(getTokenResponse.IsSuccessStatusCode);
        }

        private AuthenticationRefreshRequestModel GetAuthenticationRefreshRequestModel(string refreshToken)
        {
            return new AuthenticationRefreshRequestModel
            {
                client_id = _stravaApiSettings.Value.ClientId,
                client_secret = _stravaApiSettings.Value.ClientSecret,
                refresh_token = refreshToken,
                grant_type = "refresh_token"
            };
        }

        private AuthenticationRequestModel GetAuthenticationRequestModel(string code)
        {
            return new AuthenticationRequestModel
            {
                client_id = _stravaApiSettings.Value.ClientId,
                client_secret = _stravaApiSettings.Value.ClientSecret,
                code = code,
                grant_type = "authorization_code"
            };
        }
    }
}
