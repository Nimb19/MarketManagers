using MarketManager.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace MarketManager.Core.MarketClients;
public class TinkoffInvestApiClient : IMarketClient
{
    private readonly ILogger<TinkoffInvestApiClient> _logger;

    public const string SandboxStockFigi = "BBG004730N88"; // Сбер
    public const string SandboxCurrencyIsoCode = "RUB";

    public string Name { get; init; }
    public ClientApiName ApiName { get; init; }

    public bool IsSandboxMode { get; private set; }
    public InvestApiClient SourceApi { get; init; }
    public IOptions<TinkoffInvestApiClientOptions> Options { get; private set; }
    public MoneyValue Balance { get; private set; }
    public string AccountId { get; private set; }
    
    public TinkoffInvestApiClient(ILogger<TinkoffInvestApiClient> logger
        , InvestApiClient investApiClient, IOptions<TinkoffInvestApiClientOptions> opts)
    {
        _logger = logger;

        ApiName = ClientApiName.TinkoffInvestApi;
        SourceApi = investApiClient;

        Options = opts;
        IsSandboxMode = Options.Value.IsSandboxMode;

        _logger.LogInformation("Инициализация успешна");
    }

    public async Task Init()
    {
        var accsLength = 1;
        if (IsSandboxMode)
        {
            var accResponse = await SourceApi.Sandbox.OpenSandboxAccountAsync(new OpenSandboxAccountRequest());
            AccountId = accResponse.AccountId;

            var balanceResponse = await SourceApi.Sandbox.SandboxPayInAsync(new SandboxPayInRequest()
            {
                AccountId = AccountId,
                Amount = new MoneyValue()
                {
                    Currency = SandboxCurrencyIsoCode,
                    Units = 100000,
                }
            });
            Balance = balanceResponse.Balance;
        }
        else
        {
            var accounts = await SourceApi.Users.GetAccountsAsync();
            accsLength = accounts.Accounts.Count;
            var facc = accounts.Accounts.First();
            AccountId = facc.Id;
        }

        _logger.LogInformation($"AccountId = {AccountId} " +
            $"(accounts.Length={accsLength}, IsSandboxMode={IsSandboxMode})");
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
        // TODO: Dispose
    }
}