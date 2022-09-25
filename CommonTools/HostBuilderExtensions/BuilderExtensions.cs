using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Reflection;
using System.Text;

namespace CommonTools.HostBuilderExtensions
{
    public static class BuilderExtensions
    {
        public static IHostBuilder GymDefaultConfigure(this IHostBuilder builder)
        {
            return builder.ConfigureWithAppSettingsConfigs()
                .UseConfiguredSerilog();
        }

        public static IHostBuilder ConfigureWithAppSettingsConfigs(this IHostBuilder builder)
        {
            return builder.ConfigureAppConfiguration((app) =>
            {
                app.AddJsonFile("appsettings.json", true);
                app.AddJsonFile("appsettings.Development.json", true);
                app.AddJsonFile("appsettings.Production.json", true);
            });
        }

        public static IHostBuilder UseConfiguredSerilog(this IHostBuilder builder)
        {
            return builder.UseSerilog((context, loggingConfiguration) =>
            {
                loggingConfiguration
                    .WriteTo.Console()
                    .WriteTo.File($"{AppDomain.CurrentDomain.FriendlyName}..log"
                        , rollingInterval: RollingInterval.Day, encoding: Encoding.UTF8);
            });
        }

        public static IHost WriteInitializeMessage(this IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<object>>();

                logger.LogInformation($"{Environment.NewLine}");
                logger.LogInformation($"Start program");
                logger.LogInformation($"Version: {Assembly.GetEntryAssembly()?.GetName()?.Version.ToString() ?? "NULL"}");
            }
            return host;
        }
    }
}
