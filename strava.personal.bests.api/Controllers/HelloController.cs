using Microsoft.AspNetCore.Mvc;

namespace strava.personal.bests.api.Controllers
{
    [ApiController]
    public class HelloController : CustomApiController
    {
        [Route("hi")]
        public ActionResult Hi()
        {
            return new OkObjectResult("Hello World");
        }
    }
}