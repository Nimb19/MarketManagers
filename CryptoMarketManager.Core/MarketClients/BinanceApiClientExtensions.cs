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
            bo.LogLevel = LogLevel.Debug;
            bo.ApiCredentials = new ApiCredentials(opts.ApiKey, opts.ApiSecret);
            bo.SpotApiOptions.RateLimitingBehaviour = RateLimitingBehaviour.Fail;
            if (opts.IsSandboxMode)
                bo.SpotApiOptions.BaseAddress = BinanceApiAddresses.TestNet.RestClientAddress;
        });

        sc.AddSingleton<BinanceApiClient>();

        return sc;
    }
}
