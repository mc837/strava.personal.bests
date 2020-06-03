using FluentAssertions;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using strava.personal.bests.api.Models;
using strava.personal.bests.api.tests.Helpers;
using strava.personal.bests.api.tests.TestData;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using Xunit;

namespace strava.personal.bests.api.tests.IntegrationTests
{
    public class GetAthleteTests : IClassFixture<TestFixture>
    {
        private readonly HttpClient _client;
        private readonly FluentMockServer _stravaApiStub;
        private readonly string _validCookie;
        private readonly string _sutUrl;

        public GetAthleteTests(TestFixture fixture)
        {
            _client = fixture.Client;
            _stravaApiStub = fixture.StravaApiStub;
            _validCookie = fixture.ValidCookie;
            _sutUrl = fixture.SutUrl;
        }

        [Fact]
        public async Task GetAthlete_ShouldReturnAnAthlete_OnSuccess()
        {
            var responseContent = AthleteTestData.GetAnAthlete();
            _stravaApiStub.Given(
                    Request.Create()
                        .WithPath("/api/v3/athlete")
                        .UsingGet()
                )
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(responseContent));

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_sutUrl}/api/getAthlete");
            request.Headers.Add("Cookie", new CookieHeaderValue("spb", _validCookie).ToString());

            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var athlete = JsonConvert.DeserializeObject<Athlete>(content, new JsonSerializerSettings
            {
                ContractResolver = new IgnoreJsonPropertyAttributesForTypeResolver<Athlete>()
            });
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            athlete.Should().BeEquivalentTo(responseContent);
        }
    }
}
