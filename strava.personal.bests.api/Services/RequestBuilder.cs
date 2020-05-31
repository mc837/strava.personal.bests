using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace strava.personal.bests.api.Services
{
    public class RequestBuilder
    {
        private HttpRequestMessage _request;

        public RequestBuilder New(HttpMethod method, string url)
        {
            _request = new HttpRequestMessage(method, url);
            return this;
        }

        public RequestBuilder WithAuthHeader(string accessToken)
        {
            _request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            return this;
        }

        public RequestBuilder WithContent<T>(T content)
        {
            var body = JsonConvert.SerializeObject(content);
            _request.Content = new StringContent(body, Encoding.UTF8, "application/json");
            return this;
        }

        public HttpRequestMessage Create()
        {
            return _request;
        }
    }

}
