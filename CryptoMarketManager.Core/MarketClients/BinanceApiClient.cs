using Binance.Net.Clients;
using Binance.Net.Interfaces.Clients;
using CryptoMarketManager.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CryptoMarketManager.Core.MarketClients;

public class BinanceApiClient : ICryptoMarketClient
{
    private readonly ILogger<BinanceApiClient> _logger;

    public string Name { get; init; }
    public CryptoClientApiName ApiName { get; init; }

    public bool IsSandboxMode { get; private set; }
    public BinanceClient SourceApi { get; init; }
    public IOptions<BinanceApiClientOptions> Options { get; private set; }
    //public MoneyValue Balance { get; private set; }
    public string AccountId { get; private set; }

    public BinanceApiClient(ILogger<BinanceApiClient> logger
        , IBinanceClient сlient, IOptions<BinanceApiClientOptions> opts)
    {
        _logger = logger;

        ApiName = CryptoClientApiName.Binance;
        SourceApi = (сlient as BinanceClient) 
            ?? throw new Exception($"IBinanceClient сlient as BinanceClient = null");

        Options = opts;
        IsSandboxMode = Options.Value.IsSandboxMode;

        _logger.LogInformation("Контруктор отработал успешно");
    }

    public async Task Init()
    {
        //var accsLength = 1;
        //if (IsSandboxMode)
        //{
        //    var accResponse = await SourceApi.Sandbox.OpenSandboxAccountAsync(new OpenSandboxAccountRequest());
        //    AccountId = accResponse.AccountId;

        //    var balanceResponse = await SourceApi.Sandbox.SandboxPayInAsync(new SandboxPayInRequest()
        //    {
        //        AccountId = AccountId,
        //        Amount = new MoneyValue()
        //        {
        //            Currency = SandboxCurrencyIsoCode,
        //            Units = 100000,
        //        }
        //    });
        //    Balance = balanceResponse.Balance;
        //}
        //else
        //{
        //    var accounts = await SourceApi.Users.GetAccountsAsync();
        //    accsLength = accounts.Accounts.Count;
        //    var facc = accounts.Accounts.First();
        //    AccountId = facc.Id;
        //}

        //_logger.LogInformation("Инициализация успешна");

        //_logger.LogInformation($"AccountId = {AccountId} " +
        //    $"(accounts.Length={accsLength}, IsSandboxMode={IsSandboxMode})");
    }

    public bool Buy(ICryptoCoin coin, double price, double count)
    {
        throw new NotImplementedException();
    }

    public bool Sell(ICryptoCoin coin, double price, double count)
    {
        throw new NotImplementedException();
    }

    public CryptoClientInfo GetClientInfo()
    {
        throw new NotImplementedException();
    }

    public CryptoCoinFrameStatistic[] GetStatistic(ICryptoCoin coin, TimeSpan interval, DateTimeOffset dateFrom, DateTimeOffset dateTo)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        SourceApi?.Dispose();
    }
}
