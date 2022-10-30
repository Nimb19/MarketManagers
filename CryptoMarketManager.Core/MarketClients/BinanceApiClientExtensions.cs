using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CryptoMarketManager.Core.MarketClients;

public static class BinanceApiClientExtensions
{
    public static IServiceCollection AddBinanceApiClient(this IServiceCollection sc, BinanceApiClientOptions opts)
    {
        sc.AddBinance((bo, bso) =>
        {
            var apiCreds = new ApiCredentials(opts.ApiKey, opts.ApiSecret);

            bo.LogLevel = LogLevel.Debug;
            bo.ApiCredentials = apiCreds;
            bo.SpotApiOptions.RateLimitingBehaviour = RateLimitingBehaviour.Fail;
            if (opts.IsSandboxMode)
                bo.SpotApiOptions.BaseAddress = BinanceApiAddresses.TestNet.RestClientAddress;

            bso.LogLevel = LogLevel.Debug;
            bso.ApiCredentials = apiCreds;
            if (opts.IsSandboxMode)
                bso.SpotStreamsOptions.BaseAddress = BinanceApiAddresses.TestNet.SocketClientAddress;
        });

        sc.AddSingleton<BinanceApiClient>();

        return sc;
    }
}
