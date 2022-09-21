using Microsoft.Extensions.Logging;
using Tinkoff.InvestApi;

namespace MarketManager.Core.Models.MarketClients;

public class TinkoffInvestApiClient : IMarketClient
{
    private readonly ILogger<TinkoffInvestApiClient> _logger;
    private InvestApiClient _tincoffApi;

    public string Name { get; init; }
    public ClientApiName ApiName { get; init; }

    public TinkoffInvestApiClient(ILogger<TinkoffInvestApiClient> logger, InvestApiClient investApiClient)
    {
        _logger = logger;

        ApiName = ClientApiName.TinkoffInvestApi;
        _tincoffApi = investApiClient;

        _logger.LogInformation("Инициализация успешна");
    }

    public bool Buy(IStock stock, int count)
    {
        throw new NotImplementedException();
    }

    public bool Sell(IStock stock, int count)
    {
        throw new NotImplementedException();
    }

    public StockClientReserve[] GetReserves()
    {
        throw new NotImplementedException();
    }

    public StockFrameStats[] GetStatistic(IStock coin, TimeSpan interval, DateTimeOffset dateFrom, DateTimeOffset dateTo)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
