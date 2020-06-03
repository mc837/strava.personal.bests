using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using strava.personal.bests.api.Controllers;
using strava.personal.bests.api.Models;
using strava.personal.bests.api.Services.Interfaces;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace strava.personal.bests.api.tests.UnitTests.Controllers
{
    public class AthleteControllerTests
    {
        [Fact]
        public async Task GetAthlete_ShouldReturn200StatusCode_And_AthleteObject()
        {
            const string accessToken = "hfhkfhajkfhasjkfsjkafkasjhf";
            var mockedStravaService = new Mock<IStravaService>();
            var ctrl = new AthleteController(mockedStravaService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        Items = new Dictionary<object, object> { { "access_token", accessToken } }
                    }
                }
            };

            mockedStravaService.Setup(e => e.GetAthlete(accessToken))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("{\"firstName\": \"Matt\"}")
                }));

            var response = await ctrl.GetAthlete();
            var result = response as OkObjectResult;

            Assert.NotNull(result);
            Assert.True(result is OkObjectResult);
            Assert.IsType<Athlete>(result.Value);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task GetAthlete_ShouldReturnStatusCode400()
        {
            const string accessToken = "hfhkfhajkfhasjkfsjkafkasjhf";
            var mockedStravaService = new Mock<IStravaService>();
            var ctrl = new AthleteController(mockedStravaService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        Items = new Dictionary<object, object> { { "access_token", accessToken } }
                    }
                }
            };

            mockedStravaService.Setup(e => e.GetAthlete(accessToken))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("{\"message\": \"problem\", \"errors\":[ \"resource\": \"Athlete\",\"field\": \"access_token\",\"code\": \"invalid\"]}")
                }));

            var response = await ctrl.GetAthlete();
            var result = response as BadRequestResult;

            Assert.NotNull(result);
            Assert.True(result is BadRequestResult);
            Assert.Equal(StatusCodes.Status400BadRequest, result.StatusCode);
        }

    }
}
