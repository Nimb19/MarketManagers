using CommonTools;
using CommonTools.HostBuilderExtensions;
using MarketManager.Console;
using MarketManager.Core.MarketClients;
using MarketManager.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .GymDefaultConfigure()
    .ConfigureServices((context, services) =>
    {
        var opts = context
            .Configuration.GetSection(ConfigConstants.TinkoffInvestApiClientOptions)
            .Get<TinkoffInvestApiClientOptions>();

        services.Configure<TinkoffInvestApiClientOptions>((opt) =>
        {
            PropertyCopyrighter<TinkoffInvestApiClientOptions>.CopyAllProperties(opts, opt);
        });
        services.AddTinkoffClient(opts);

        services.AddHostedService<DebugService>();
    })
    .Build()
    .WriteInitializeMessage()
    .Run();
