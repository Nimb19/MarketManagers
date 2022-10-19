using Binance.Net;
using Binance.Net.Objects;
using CommonTools;
using CommonTools.HostBuilderExtensions;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoMarketManager.Console;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

Host.CreateDefaultBuilder(args)
    .GymDefaultConfigure()
    .ConfigureServices((context, services) =>
    {
        var opts = context.GetAndConfigureOptionsFromConfig<CryptoMarketManager.Core.Models.BinanceApiClientOptions>(services
            , ConfigConstants.BinanceApiClientOptions);

        services.AddBinance((bo, bso) =>
        {
            bo.LogLevel = LogLevel.Debug;
            bo.ApiCredentials = new ApiCredentials(opts.ApiKey, opts.ApiSecret);
            bo.SpotApiOptions.RateLimitingBehaviour = RateLimitingBehaviour.Fail;
            if (opts.IsSandboxMode)
                bo.SpotApiOptions.BaseAddress = BinanceApiAddresses.TestNet.RestClientAddress;
        });
        //services.AddHostedService<DebugService>();
    })
    .Build()
    .WriteInitializeMessage()
    .Run();