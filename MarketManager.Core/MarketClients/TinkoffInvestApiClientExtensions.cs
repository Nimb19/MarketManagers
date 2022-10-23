using Grpc.Net.ClientFactory;
using Microsoft.Extensions.DependencyInjection;

namespace MarketManager.Core.MarketClients;

public static class TinkoffInvestApiClientExtensions
{
    public static readonly string DefaulGrpcClientName = AppDomain.CurrentDomain.FriendlyName + ".TinkoffInvestApiClient";
    public const string SandboxUriPath = "https://sandbox-invest-public-api.tinkoff.ru:443";

    public static IServiceCollection AddTinkoffInvestApiClient(this IServiceCollection sc, TinkoffInvestApiClientOptions opts)
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
