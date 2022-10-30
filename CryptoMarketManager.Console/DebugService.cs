using Binance.Net.Enums;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoMarketManager.Core.MarketClients;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Concurrent;

namespace CryptoMarketManager.Console;

public class DebugService : BackgroundService
{
    public const string SYMBOL = "BTCUSDT";
    private readonly BinanceApiClient m_client;
    private readonly BlockingCollection<IBinanceKline> m_klineCollection = new BlockingCollection<IBinanceKline>();

    private readonly ILogger<DebugService> m_logger;
    private readonly IHostApplicationLifetime m_lifetime;

    public DebugService(BinanceApiClient client, ILogger<DebugService> logger, IHostApplicationLifetime lifetime)
    {
        m_logger = logger;
        m_lifetime = lifetime;
        m_client = client;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var cancTokenSource = new CancellationTokenSource(/*TimeSpan.FromSeconds(130)*/);
            var ct = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, cancTokenSource.Token).Token;

            await m_client.Init(ct).ConfigureAwait(false);
            await Logica(ct).ConfigureAwait(false);
        }
        catch (OperationCanceledException exc)
        {
            m_logger.LogInformation($"OperationCanceledException: {exc}");
        }
        catch (Exception exc)
        {
            m_logger.LogError($"Ошибка внутри: {exc}");
        }
        m_logger.LogInformation($"End");
        m_lifetime.StopApplication();
    }

    private async Task Logica(CancellationToken ct)
    {
        var last20MinsKlinesResult = await m_client.SourceApi.SpotApi.ExchangeData.GetKlinesAsync(SYMBOL, KlineInterval.OneMinute
            , DateTime.UtcNow.AddMinutes(-120), ct: ct);

        var subTaskResult = await SubscribeToMinsKlines(ct);
        await ConsumeToNewKlines(ct);

        await subTaskResult.Data.CloseAsync();
        m_logger.LogInformation($"Logica cancelled");
    }

    private async Task<CallResult<UpdateSubscription>> SubscribeToMinsKlines(CancellationToken ct)
    {
        return await m_client.SourceSocketApi.SpotStreams.SubscribeToKlineUpdatesAsync(SYMBOL, KlineInterval.OneMinute,
            (klineDataEvent) =>
            {
                if (ct.IsCancellationRequested)
                {
                    if (!m_klineCollection.IsAddingCompleted)
                        m_klineCollection.CompleteAdding();
                    return;
                }

                var kline = klineDataEvent?.Data?.Data;
                m_logger.LogDebug($"{JsonConvert.SerializeObject(klineDataEvent)}");
                if (kline.Final)
                    m_klineCollection.Add(kline);
            }).ConfigureAwait(false);
    }

    private async Task ConsumeToNewKlines(CancellationToken ct)
    {
        var last2MinsKlinesResult = await m_client.SourceApi.SpotApi.ExchangeData.GetKlinesAsync(SYMBOL, KlineInterval.OneMinute
                    , DateTime.UtcNow.AddMinutes(-2), ct: ct).ConfigureAwait(false);
        if (!last2MinsKlinesResult.Success)
            throw new Exception(last2MinsKlinesResult.Error?.ToString());

        var last2MinsKlinesArray = last2MinsKlinesResult.Data ?? throw new ArgumentNullException($"{nameof(last2MinsKlinesResult)}.Data = null");
        var avgX2KlineForMins = await Get2xAvgMinuteKlineAsync(ct).ConfigureAwait(false);

        var lastClosePrice = last2MinsKlinesArray.First().ClosePrice;
        foreach (var newKline in m_klineCollection.GetConsumingEnumerable())
        {
            if (ct.IsCancellationRequested)
            {
                if (!m_klineCollection.IsAddingCompleted)
                    m_klineCollection.CompleteAdding();
                break;
            }

            var isVolumeHigherThen2xAvg = avgX2KlineForMins.Volume < newKline.Volume;
            var isPriceUp = lastClosePrice < newKline.ClosePrice;

            m_logger.LogInformation($"[VOLUME] Пришла свечка: {newKline.Volume} (closePrice={newKline.ClosePrice}). " +
                $"Средняя: {avgX2KlineForMins.Volume}.{Environment.NewLine}" +
                $"{nameof(isVolumeHigherThen2xAvg)}={isVolumeHigherThen2xAvg} (= {avgX2KlineForMins.Volume} > {newKline.Volume}).{Environment.NewLine}" +
                $"{nameof(isPriceUp)}={isPriceUp} (= {lastClosePrice} > {newKline.ClosePrice}).{Environment.NewLine}" +
                $"{(isVolumeHigherThen2xAvg && isPriceUp ? "БЕРУ!!!" : "НЕ ВОЗЬМУ")}" +
                $"{Environment.NewLine}{JsonConvert.SerializeObject(newKline, Formatting.Indented)}");

            lastClosePrice = newKline.ClosePrice;

            //m_client.SourceApi.SpotApi.Trading.;
        }
    }

    private async Task<BinanceSpotKline> Get2xAvgMinuteKlineAsync(CancellationToken cancellationToken, decimal x = 2)
    {
        var lastWeakResult = await m_client.SourceApi.SpotApi.ExchangeData.GetKlinesAsync(SYMBOL, KlineInterval.OneDay
            , DateTime.UtcNow.AddDays(-7), ct: cancellationToken).ConfigureAwait(false);

        if (!lastWeakResult.Success)
            throw new Exception(lastWeakResult.Error?.ToString());
        var lastWeakArray = lastWeakResult.Data ?? throw new ArgumentNullException($"{nameof(lastWeakResult)}.Data = null");

        var avgForMins = new BinanceSpotKline();
        foreach (var dayKline in lastWeakArray)
        {
            avgForMins.TradeCount += dayKline.TradeCount;
            avgForMins.Volume += dayKline.Volume;
            avgForMins.QuoteVolume += dayKline.QuoteVolume;
        }
        var minutesCount = lastWeakArray.Count() * 24 * 60;
        avgForMins.TradeCount /= minutesCount;
        avgForMins.Volume /= minutesCount;
        avgForMins.QuoteVolume /= minutesCount;
        m_logger.LogInformation($"{nameof(avgForMins)}={JsonConvert.SerializeObject(avgForMins, Formatting.Indented)}");

        avgForMins.TradeCount = Convert.ToInt32(avgForMins.TradeCount * x);
        avgForMins.Volume *= x;
        avgForMins.QuoteVolume *= x;
        m_logger.LogInformation($"x2Avg={JsonConvert.SerializeObject(avgForMins, Formatting.Indented)}");
        return avgForMins;
    }
}
