using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Metronome.Api
{
    public class Startup_Cors : IStartup
    {

        readonly IConfiguration _settings;

        public Startup_Cors(IConfiguration settings)
        {
            _settings = settings;
        }

        public void ConfigureServices(IServiceCollection services)
        {

        }

        public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(c =>
            {
                c.AllowAnyHeader();
                c.AllowAnyMethod();
                c.AllowCredentials();
                c.WithOrigins(_settings["AllowedHosts"]);
            });
        }
    }
}
