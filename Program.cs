using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebCore.ApiPhones
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Api.Phones";

            //Read Configuration from appSettings
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            //Initialize Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            try
            {
                Log.Information("Application Starting.");
                var host = CreateWebHostBuilder(args).Build();
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Loopback, 5016);
                })
                .UseStartup<Startup>()
                .UseSerilog(); //Uses Serilog instead of default .NET Logger

                        //.UseSerilog((context, configuration) =>  
                        //{
                        //    configuration
                        //        .MinimumLevel.Debug()
                        //        .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                        //        .MinimumLevel.Override("System", LogEventLevel.Warning)
                        //        .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                        //        .Enrich.FromLogContext()
                        //        .WriteTo.File(@"api_phonebook_log.txt")
                        //        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate);
                        //});
    }
}

