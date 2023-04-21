using System;
using System.Globalization;
using System.IO;
using Codecool.CodecoolShop.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Configuration;
using Serilog;
using Serilog.Filters;
using Serilog.Formatting.Json;

namespace Codecool.CodecoolShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var newGuid = Guid.NewGuid();
            FilePath.Path = Path.Combine(Environment.CurrentDirectory, "Data", "Log", $"{newGuid}.json");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .WriteTo.Console(new JsonFormatter())
                .WriteTo.File(new JsonFormatter(), FilePath.Path)
                .Filter.ByIncludingOnly(evt => evt.Properties.ContainsKey("SourceContext") && evt.Properties["SourceContext"].ToString().Contains("CartController"))
                .CreateLogger();

            try
            {
                Log.Information("Application Starting Up");
                CreateHostBuilder(args).Build().Run();

          
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Cos nie dziala kierowniku");
                throw;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
