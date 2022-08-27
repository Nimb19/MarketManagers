namespace MarketManager.Core.Models
{
    public interface IMarketClient : IDisposable
    {
        public string Name { get; init; }
        public ClientApiName ApiName { get; init; }

        public bool Buy(IStock stock, int count);
        public bool Sell(IStock stock, int count);

        public StockClientReserve[] GetReserves();
        public StockFrameStats[] GetStatistic(IStock coin, TimeSpan interval, DateTimeOffset dateFrom, DateTimeOffset dateTo);
    }

    public enum ClientApiName
    {
        TinkoffInvestApi = 1,
    }

    public class StockClientReserve
    {
        public IStock Stock { get; init; }
        public int Reserve { get; init; }

        public StockClientReserve(IStock stock, int reserve)
        {
            Stock = stock;
            Reserve = reserve;
        }
    }

    public class StockFrameStats
    {
        public IStock Coin { get; init; }
        public DateTimeOffset FrameDate { get; init; }
        public double Price { get; init; }
    }
}
