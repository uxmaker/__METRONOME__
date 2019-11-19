// SYSTEM

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// MICROSOFT

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Metronome.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHostBuilder constructor = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((webBuilderContext , appConfigBuilder) =>
                    {
                        appConfigBuilder    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
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
