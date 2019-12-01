using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Metronome.Api.Authentication.Cookie;
using Metronome.Api.Authentication.Jwt;

namespace Metronome.Api
{
    public class Startup_Authentication : IStartup
    {
        readonly IConfiguration _settings;
        readonly SymmetricSecurityKey _signingKey;

        public Startup_Authentication(IConfiguration settings)
        {
            _settings = settings;
            _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings["Authentication:SecretKey"]));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AuthTokenOptions>(options =>
            {
                options.Audience = _settings["JwtBearer:Audience"];
                options.Issuer = _settings["JwtBearer:Issuer"];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(AuthCookieParameters.Scheme)
                    .AddCookie(AuthCookieParameters.Scheme, opts =>
                    {
                        opts.ExpireTimeSpan = TimeSpan.FromHours(1);
                        opts.SlidingExpiration = true;
                    })
                    .AddJwtBearer(AuthTokenParameters.Scheme, opts =>
                    {
                        opts.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,

                            ValidIssuer = _settings["Authentication:Issuer"],
                            IssuerSigningKey = _signingKey,
                            ValidAudience = _settings["Authentication:Audience"],
                            AuthenticationType = AuthTokenParameters.Type,
                            NameClaimType = ClaimTypes.Email
                        };
                    });
        }

        public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
        }
    }
}
