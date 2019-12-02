using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Metronome.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHostBuilder constructor = new WebHostBuilder()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((webBuilderContext, appConfigBuilder) =>
               {
                   appConfigBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                    .AddEnvironmentVariables()
                                    .AddCommandLine(args);
               })
               .ConfigureLogging((webBuilderContext, appLoggingBuilder) =>
               {
                   appLoggingBuilder.AddConsole();
               })
               .UseKestrel()
               .UseIISIntegration()
               .UseStartup<Startup>();

            constructor.Build().Run();
        }
    }
}
