using System.Linq;
using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Spot;
using CryptoMarketManager.Core.MarketClients;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CryptoMarketManager.Console;

public class DebugService : BackgroundService
{
    //private readonly BinanceSocketClient _socClient;
    private readonly BinanceApiClient _client;

    private readonly ILogger<DebugService> _logger;
    private readonly IHostApplicationLifetime _lifetime;

    public const string SYMBOL = "BTCUSDT";

    public DebugService(BinanceApiClient client, ILogger<DebugService> logger, IHostApplicationLifetime lifetime)
    {
        _logger = logger;
        _lifetime = lifetime;
        _client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var cancTokenSource = new CancellationTokenSource(/*TimeSpan.FromSeconds(130)*/);
            var combToken = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, cancTokenSource.Token).Token;

            await _client.Init();
            await Logica(combToken);
        }
        catch (Exception exc)
        {
            _logger.LogError($"Ошибка внутри: {exc}");
        }
        _logger.LogInformation($"End");
        _lifetime.StopApplication();
    }

    private async Task Logica(CancellationToken cancellationToken)
    {
        var last20MinsKlinesResult = await _client.SourceApi.SpotApi.ExchangeData.GetKlinesAsync(SYMBOL, KlineInterval.OneMinute
            , DateTime.UtcNow.AddMinutes(-20), ct: cancellationToken);
       
        if (!last20MinsKlinesResult.Success)
            throw new Exception(last20MinsKlinesResult.Error?.ToString());
        var last20MinsKlinesArray = last20MinsKlinesResult.Data ?? throw new ArgumentNullException($"last20MinsKlinesResult.Data = null");
        var avgFromLast3MounthsForMins = await GetAvgMinuteKlineAsync(cancellationToken);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            
        }

        _logger.LogInformation($"Logica cancelled");
    }

    private async Task<BinanceSpotKline> GetAvgMinuteKlineAsync(CancellationToken cancellationToken)
    {
        var last12WeaksResult = await _client.SourceApi.SpotApi.ExchangeData.GetKlinesAsync(SYMBOL, KlineInterval.OneWeek
            , DateTime.UtcNow.AddMonths(-3), ct: cancellationToken);

        if (!last12WeaksResult.Success)
            throw new Exception(last12WeaksResult.Error?.ToString());
        var last12WeaksArray = last12WeaksResult.Data ?? throw new ArgumentNullException($"last12WeaksResult.Data = null");

        var avgFromLast3MounthsForMins = new BinanceSpotKline();
        foreach (var weakKline in last12WeaksArray)
        {
            avgFromLast3MounthsForMins.TradeCount += weakKline.TradeCount;
            avgFromLast3MounthsForMins.Volume = weakKline.Volume;
            avgFromLast3MounthsForMins.QuoteVolume = weakKline.QuoteVolume;
        }
        var minutesCount = last12WeaksArray.Count() * 7 * 24 * 60;
        avgFromLast3MounthsForMins.TradeCount /= minutesCount;
        avgFromLast3MounthsForMins.Volume /= minutesCount;
        avgFromLast3MounthsForMins.QuoteVolume /= minutesCount;
        _logger.LogInformation($"avgFromLast3MounthsForMins={JsonConvert.SerializeObject(avgFromLast3MounthsForMins, Formatting.Indented)}");
        return avgFromLast3MounthsForMins;
    }
}
