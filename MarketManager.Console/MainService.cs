using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

namespace MarketManager.Console
{
    public class MainService : BackgroundService
    {
        private readonly InvestApiClient _investApiClient;
        private readonly ILogger<MainService> _logger;
        private readonly IHostApplicationLifetime _lifetime;

        public MainService(InvestApiClient investApiClient, ILogger<MainService> logger, IHostApplicationLifetime lifetime)
        {
            _logger = logger;
            _lifetime = lifetime;
            _investApiClient = investApiClient;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var accounts = await _investApiClient.Users.GetAccountsAsync();
            var accountId = accounts.Accounts.First().Id;
            _logger.LogInformation($"accountId = {accountId}");

            var stockInfo = _investApiClient.MarketData.GetLastTrades();

            _lifetime.StopApplication();
        }
    }
}
