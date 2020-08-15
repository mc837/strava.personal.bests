using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using strava.personal.bests.api.Models;
using strava.personal.bests.api.Services;
using strava.personal.bests.api.Services.Interfaces;

namespace strava.personal.bests.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpClient();
            services.AddScoped<ICrypto, Crypto>();
            services.Configure<StravaApiSettings>(Configuration.GetSection("stravaApiSettings"));
            services.Configure<StravaPersonalBestsSettings>(Configuration.GetSection("StravaPersonalBestsSettings"));
            services.AddScoped<IStravaAuthService, StravaAuthService>();
            services.AddScoped<IStravaService, StravaService>();
            services.AddCors(options =>
            {
                options.AddPolicy("Policy1",
                    builder => { builder.AllowCredentials().WithOrigins("https://localhost:3000").AllowAnyHeader(); });
            });
            //TODO: Check scoping
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
