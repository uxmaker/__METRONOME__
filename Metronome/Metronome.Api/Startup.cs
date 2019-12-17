using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;


namespace Metronome.Api
{
    public class Startup
    {
        public IConfiguration Settings { get; }
        readonly IStartup _mvc;
        readonly IStartup _authentication;
        readonly IStartup _injector;
        readonly IStartup _cors;

        public Startup(IConfiguration settings)
        {
            Settings = settings;
            _mvc = new Startup_Mvc(Settings);
            _authentication = new Startup_Authentication(Settings);
            _injector = new Startup_Injector(Settings);
            _cors = new Startup_Cors(Settings);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            _injector.ConfigureServices(services);
            _authentication.ConfigureServices(services);
            _mvc.ConfigureServices(services);
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()){ app.UseDeveloperExceptionPage(); }
            _cors.ConfigureApp(app, env);
            _authentication.ConfigureApp(app, env);
            _mvc.ConfigureApp(app, env);

            var contentTypeProvider = new FileExtensionContentTypeProvider();
            contentTypeProvider.Mappings[".geojson"] = "application/json";
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Maps")),
                RequestPath = "/Maps",
                ContentTypeProvider = contentTypeProvider
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")),
                RequestPath = "/Images",
                ContentTypeProvider = new FileExtensionContentTypeProvider()

            });
        }
    }
}
