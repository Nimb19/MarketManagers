using CommonTools.HostBuilderExtensions;
using MarketManager.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .GymDefaultConfigure()
    .ConfigureServices((context, services) =>
    {
        services.AddInvestApiClient((_, settings) =>
        {
            context.Configuration.Bind(settings);
            settings.AppName = AppDomain.CurrentDomain.FriendlyName;
        });
        services.AddHostedService<MainService>();
    })
    .Build()
    .WriteInitializeMessage()
    .Run();