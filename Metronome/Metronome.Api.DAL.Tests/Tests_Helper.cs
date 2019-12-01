using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Metronome.Api.DAL.Tests
{
    public static class Tests_Helper
    {

        static readonly Random _random = new Random();
        static IConfiguration _settings;

        static IConfiguration Settings
        {
            get
            {
                if( _settings == null )
                {
                    _settings = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json", optional: true)
                                    .AddEnvironmentVariables()
                                    .Build();
                }
                return _settings;
            }
        }

        public static string ConnectionString
        {
            get
            {
                // return Settings["SQL:ConnectionString"]
                return "Server=DESKTOP-JC6TRMR\\SQLEXPRESS;Database=METRONOME;Trusted_Connection=True";
            }
        }

        public static string RandomString(string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix)) prefix = "test";
            return string.Format("{0}_{1}", prefix, Guid.NewGuid().ToString().Substring(24));
        }

    }
}
