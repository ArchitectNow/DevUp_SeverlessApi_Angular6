using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace ArchitectNow.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
           try
            {
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((ctx, builder) =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                        .Enrich.FromLogContext()
                        .WriteTo.File("Logs/log.txt", restrictedToMinimumLevel: LogEventLevel.Warning)
                        .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Debug)
                        .CreateLogger();
                    
                })
                .UseStartup<Startup>()
                .UseKestrel()
                .UseSerilog()
                .Build();
    }
}