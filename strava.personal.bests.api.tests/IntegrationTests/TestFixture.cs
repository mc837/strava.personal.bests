using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using strava.personal.bests.api.Models.Authentication;
using strava.personal.bests.api.Services;
using System;
using System.IO;
using System.Net.Http;
using WireMock.Logging;
using WireMock.Server;
using WireMock.Settings;

namespace strava.personal.bests.api.tests.IntegrationTests
{
    public class TestFixture : IDisposable
    {
        private static readonly string ProjectDir = Directory.GetCurrentDirectory();
        private readonly string _configPath = Path.Combine(ProjectDir, "IntegrationTests/appsettings.json");
        public string ValidCookie { get; set; }
        public HttpClient Client { get; set; }
        public TestServer Server { get; }
        public FluentMockServer StravaApiStub { get; }
        public string SutUrl => "http://localhost:5001";

        public TestFixture()
        {
            var crypto = new Crypto();

            ValidCookie = crypto.Encrypt("test", JsonConvert.SerializeObject(new StravaTokenModel
            {
                AccessToken = "ceecf40e0aca4babaa3c8719f4e6f890",
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(5).ToUnixTimeSeconds().ToString()
            }));

            var builder = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((ctx, conf) => { conf.AddJsonFile(_configPath); });

            Server = new TestServer(builder);
            Client = Server.CreateClient();
            Client.BaseAddress = new Uri(SutUrl);

            StravaApiStub = FluentMockServer.Start(new FluentMockServerSettings
            {
                Urls = new[] { "http://localhost:8001" },
                StartAdminInterface = true,
                Logger = new WireMockConsoleLogger(),
            });
        }

        public void Dispose()
        {
            StravaApiStub?.Stop();
            Client?.Dispose();
            Server?.Dispose();
        }
    }
}
