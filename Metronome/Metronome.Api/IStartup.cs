using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Metronome.Api
{
    public interface IStartup
    {
        public void ConfigureServices(IServiceCollection services);
        public void ConfigureApp(IApplicationBuilder app, IWebHostEnvironment env);
    }
}
