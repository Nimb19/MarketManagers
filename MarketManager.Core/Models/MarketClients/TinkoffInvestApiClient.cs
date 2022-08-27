using Tinkoff.InvestApi;

namespace MarketManager.Core.Models.MarketClients
{
    public class TinkoffInvestApiClient : IMarketClient
    {
        private InvestApiClient _tincoffApi;

        public string Name { get; init; }
        public ClientApiName ApiName { get; init; }

        public TinkoffInvestApiClient(string name)
        {
            ApiName = ClientApiName.TinkoffInvestApi;

            _tincoffApi = ;
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
}
