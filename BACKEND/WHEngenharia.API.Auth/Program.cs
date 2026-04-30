using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace WHEngenharia.API.Auth
{
    public class Program
    {
        private const string AspNetCoreEnvironment = "ASPNETCORE_ENVIRONMENT";

        public static void Main(string[] args)
        {
            BuildWebHost().Run();
        }

        private static IWebHost BuildWebHost()
        {
            var basePath = Directory.GetCurrentDirectory();

            var urls = new[]
            {
                "http://*:5501",
            };

            return WebHost.CreateDefaultBuilder()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureAppConfiguration((context, options) =>
                {
                    options.Sources.Clear();

                    options.SetBasePath(basePath)
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable(AspNetCoreEnvironment)}.json",
                            optional: true)
                        .AddEnvironmentVariables()
                        .Build();
                })
                .UseIIS()
                .UseUrls(urls)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
