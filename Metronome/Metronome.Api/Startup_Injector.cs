using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Metronome.Api.DAL.Member;
using Metronome.Api.Authentication;
using Metronome.Api.Authentication.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Metronome.Api
{
    public class Startup_Injector : IStartup
    {

        readonly IConfiguration _settings;

        public Startup_Injector(IConfiguration settings)
        {
            _settings = settings;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(_ => new MemberGateway(_settings["SQL:ConnectionString"]));
            services.AddSingleton(_ => new PasswordHasher());
            services.AddSingleton<AuthTokenService>();
        }

        public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env) { }

    }
}
