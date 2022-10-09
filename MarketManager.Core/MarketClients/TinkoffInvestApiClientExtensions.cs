using Google.Protobuf.WellKnownTypes;
using Grpc.Net.ClientFactory;
using MarketManager.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;

namespace MarketManager.Core.MarketClients;

public static class TinkoffInvestApiClientExtensions
{
    public static readonly string DefaulGrpcClientName = AppDomain.CurrentDomain.FriendlyName + ".TinkoffInvestApiClient";
    public const string SandboxUriPath = "https://sandbox-invest-public-api.tinkoff.ru:443";

    public static IServiceCollection AddTinkoffClient(this IServiceCollection sc, TinkoffInvestApiClientOptions opts)
    {
        var grpcClientName = opts.GrpcClientName ?? DefaulGrpcClientName;

        sc.AddInvestApiClient(grpcClientName, (sp, settings) =>
        {
            settings.AccessToken = opts.AccessToken;
            settings.AppName = opts.AppName;
        });

        if (opts.IsSandboxMode)
        {
            sc.Configure<GrpcClientFactoryOptions>(grpcClientName, (opts) =>
            {
                opts.Address = new Uri(SandboxUriPath);
            });
        }

        return sc.AddSingleton<TinkoffInvestApiClient>();
    }
}
