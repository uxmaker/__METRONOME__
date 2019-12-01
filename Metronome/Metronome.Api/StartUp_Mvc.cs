using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Metronome.Api
{
    public class Startup_Mvc : IStartup
    {
        readonly IConfiguration _settings;

        public Startup_Mvc(IConfiguration settings)
        {
            _settings = settings;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddMvc()
                    .AddMvcOptions((options) => { options.EnableEndpointRouting = false; });
        }

        public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMvc(
                routeBuilder =>
                {
                    routeBuilder.MapRoute(
                        name: "default",
                        template: "{controller}/{action}/{id?}",
                        defaults: new { controller = "Auth", action = "Login" });
                });
        }

    }
}
