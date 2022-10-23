using CommonTools.HostBuilderExtensions;
using CryptoMarketManager.Console;
using CryptoMarketManager.Core.MarketClients;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .GymDefaultConfigure()
    .ConfigureServices((context, services) =>
    {
        var opts = context.GetAndConfigureOptionsFromConfig<CryptoMarketManager.Core.MarketClients.BinanceApiClientOptions>(services
            , ConfigConstants.BinanceApiClientOptions);

        services.AddBinanceApiClient(opts);
        services.AddHostedService<DebugService>();
    })
    .Build()
    .WriteInitializeMessage()
    .Run();