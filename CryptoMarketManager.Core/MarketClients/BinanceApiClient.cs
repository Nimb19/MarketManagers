using Binance.Net.Clients;
using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects.Models.Spot;
using CryptoMarketManager.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace CryptoMarketManager.Core.MarketClients;

public class BinanceApiClient : ICryptoMarketClient
{
    private readonly ILogger<BinanceApiClient> m_logger;

    public string Name { get; init; }
    public CryptoClientApiName ApiName { get; init; }

    public bool IsSandboxMode { get; private set; }
    public BinanceClient SourceApi { get; init; }
    public BinanceSocketClient SourceSocketApi { get; }
    public IOptions<BinanceApiClientOptions> Options { get; private set; }
    //public MoneyValue Balance { get; private set; }

    public BinanceAccountInfo BinanceAccountInfo { get; protected set; }

    public BinanceApiClient(ILogger<BinanceApiClient> logger
        , IBinanceClient сlient, IBinanceSocketClient socketClient, IOptions<BinanceApiClientOptions> opts)
    {
        m_logger = logger;

        ApiName = CryptoClientApiName.Binance;
        SourceApi = (сlient as BinanceClient) 
            ?? throw new Exception($"IBinanceClient сlient as BinanceClient = null");
        SourceSocketApi = (socketClient as BinanceSocketClient)
            ?? throw new Exception($"IBinanceSocketClient сlient as BinanceSocketClient = null");

        Options = opts;
        IsSandboxMode = Options.Value.IsSandboxMode;

        m_logger.LogInformation("Контруктор отработал успешно");
    }

    public async Task<bool> Init(CancellationToken ct)
    {
        var accInfo = await SourceApi.SpotApi.Account.GetAccountInfoAsync().ConfigureAwait(false);
        if (accInfo.Success)
        {
            m_logger.LogInformation($"[INIT_SUCCESS] {nameof(BinanceApiClient)}");
            m_logger.LogDebug($"AccountInfo = {JsonConvert.SerializeObject(accInfo.Data, Formatting.Indented)}");
            BinanceAccountInfo = accInfo.Data ?? throw new Exception($"SourceApi.SpotApi.Account.GetAccountInfoAsync().Data is NULL");
        }
        else
            m_logger.LogError($"SourceApi.SpotApi.Account.GetAccountInfoAsync() is not success. Error: " +
                $"{Environment.NewLine}{accInfo.Error}");
        return accInfo.Success;
    }

    public bool Buy(ICryptoCoin coin, double price, double count, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public bool Sell(ICryptoCoin coin, double price, double count, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public CryptoClientInfo GetClientInfo()
    {
        throw new NotImplementedException();
    }

    public CryptoCoinFrameStatistic[] GetStatistic(ICryptoCoin coin, TimeSpan interval, DateTimeOffset dateFrom, DateTimeOffset dateTo, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        SourceApi?.Dispose();
    }
}
