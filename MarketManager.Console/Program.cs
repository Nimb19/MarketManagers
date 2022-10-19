using CommonTools.HostBuilderExtensions;
using MarketManager.Console;
using MarketManager.Core.MarketClients;
using MarketManager.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .GymDefaultConfigure()
    .ConfigureServices((context, services) =>
    {
        var opts = context.GetAndConfigureOptionsFromConfig<TinkoffInvestApiClientOptions>(services
            , ConfigConstants.TinkoffInvestApiClientOptions);
        services.AddTinkoffClient(opts);

        services.AddHostedService<DebugService>();
    })
    .Build()
    .WriteInitializeMessage()
    .Run();
