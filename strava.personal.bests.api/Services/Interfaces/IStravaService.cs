﻿using System.Net.Http;
using System.Threading.Tasks;

namespace strava.personal.bests.api.Services.Interfaces
{
    public interface IStravaService
    {
        Task<HttpResponseMessage> GetToken<T>(T request);
        Task<HttpResponseMessage> GetAthlete(string accessToken);
        Task<HttpResponseMessage> Deauthorize(string accessToken);
    }
}
