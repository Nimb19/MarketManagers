using Grpc.Core;
using MarketManager.Core.MarketClients;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tinkoff.InvestApi.V1;

namespace MarketManager.Console;

public class DebugService : BackgroundService
{
    private readonly TinkoffInvestApiClient _client;
    private string AccoutId = null;

    private readonly ILogger<DebugService> _logger;
    private readonly IHostApplicationLifetime _lifetime;

    public DebugService(TinkoffInvestApiClient investApiClient, ILogger<DebugService> logger, IHostApplicationLifetime lifetime)
    {
        _logger = logger;
        _lifetime = lifetime;
        _client = investApiClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await _client.Init();
            await Logica(stoppingToken);
        }
        catch (Exception exc)
        {
            _logger.LogError($"Ошибка внутри: {exc}");
        }
        _logger.LogInformation($"End");
        _lifetime.StopApplication();
    }

    private async Task Logica(CancellationToken stoppingToken)
    {
        var cancTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(130));
        var combToken = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, cancTokenSource.Token).Token;

        var stream = _client.SourceApi.MarketDataStream.MarketDataStream();

        var readTask = ReadStreamAndWriteInLoggerAsync(stream, combToken);
        var writeTask = WriteSubscribeCmdAsync(stream, combToken);

        await readTask;
        await writeTask;
    }

    private async Task ReadStreamAndWriteInLoggerAsync(AsyncDuplexStreamingCall<MarketDataRequest, MarketDataResponse> stream, CancellationToken combToken)
    {
        await foreach (var response in stream.ResponseStream.ReadAllAsync(combToken))
        {
            _logger.LogInformation($"response = {JsonConvert.SerializeObject(response, Formatting.Indented)}");
        }
        _logger.LogInformation($"readTask END");
    }

    private async Task WriteSubscribeCmdAsync(AsyncDuplexStreamingCall<MarketDataRequest, MarketDataResponse> stream, CancellationToken combToken)
    {
        await stream.RequestStream.WriteAsync(new MarketDataRequest()
        {
            SubscribeCandlesRequest = new SubscribeCandlesRequest()
            {
                Instruments =
                    {
                        new CandleInstrument()
                        {
                            Figi = "BBG000BBR9P6",
                            Interval = SubscriptionInterval.OneMinute,
                        }
                    },
                SubscriptionAction = SubscriptionAction.Subscribe,
            }
        });
    }
}
