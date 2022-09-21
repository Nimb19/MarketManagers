using MarketManager.Core.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace MarketManager.Core;

public class MarketManager
{
    private readonly ILogger<MarketManager> _logger;
    private readonly MarketManagerSettings _settings;

    public IMarketClient[] MarketClients { get; private set; }

    public MarketManager(ILogger<MarketManager> logger, MarketManagerSettings settings)
    {
        _logger = logger;
        _settings = settings;
        InitClients();
    }

    public void InitClients()
    {
        foreach (var clientConfig in _settings.CreateClientsList)
        {
            IMarketClient marketClient = null;

            switch (clientConfig)
            {
                case ClientApiName.TinkoffInvestApi:
                    break;
                default:
                    break;
            }
        }
    }
}

public class MarketManagerSettings
{
    public List<ClientApiName> CreateClientsList { get; set; }
}

public static class MarketManagerExtensions
{
    public static IServiceCollection AddMarketManager(this IServiceCollection services
        , Action<MarketManagerSettings> action)
    {
        var managerSettings = new MarketManagerSettings();
        action(managerSettings);
        if (managerSettings.CreateClientsList is null)
            throw new ArgumentNullException(nameof(managerSettings.CreateClientsList));
        else if (!managerSettings.CreateClientsList.Any())
            throw new ArgumentException("CreateClientsList не может быть пустым, сконфигурируйте необходиых клиентов"
                , nameof(managerSettings.CreateClientsList));

        services.AddSingleton(managerSettings);
        services.AddSingleton<MarketManager>();

        return services;
    }
}