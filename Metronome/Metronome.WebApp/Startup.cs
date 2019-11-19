// SYSTEM

using System;
using System.Text;
using System.Security.Claims;

// MICROSOFT

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

// METRONOME

using Metronome.WebApp.Core.Auth;

namespace Metronome.WebApp
{
    public class Startup
    {

        IConfiguration _configuration;

        public Startup( IConfiguration configuration )
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // SECRET KEY DEV ( !!! A CHANGER !!! )
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DEVELOPPEMENT-TEST"));

            // MVC
            services.AddMvc()
                    .AddMvcOptions((options) =>
                        {
                            options.EnableEndpointRouting = false;
                        });

            services.AddAuthentication(MemberCookie.AuthScheme)
                    .AddCookie(MemberCookie.AuthScheme, (options) =>
                       {
                           options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                           options.SlidingExpiration = true;
                       })
                    .AddJwtBearer(MemberTokenJwt.AuthScheme, (options) =>
                       {
                           options.TokenValidationParameters = new TokenValidationParameters
                           {
                               ValidateIssuerSigningKey = true,
                               IssuerSigningKey = signingKey,
                               ValidateIssuer = true,
                               ValidIssuer = "xxxx",
                               ValidateAudience = true,
                               ValidAudience = "xxx",
                               NameClaimType = ClaimTypes.Email,
                               ValidateLifetime = true
                           };
                       });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            // CORS
            app.UseCors((corsPolicyBuilder) =>
                {
                    corsPolicyBuilder   .AllowAnyMethod()
                                        .AllowAnyHeader()
                                        .AllowCredentials()
                                        .WithOrigins("http://localhost:8080");
                });

            // AUTH
            app.UseAuthentication();

            // MVC
            app.UseMvc();

            // STATIC FILES
            app.UseStaticFiles();

        }
    }
}
