namespace MarketManager.Core.Models
{
    public interface IMarketClient : IDisposable
    {
        public StockFrameStats[] GetStatistic(IStock coin, TimeSpan interval, DateTimeOffset dateFrom, DateTimeOffset dateTo);
        public bool Buy(IStock stock, double price, int count);
        public bool Sell(IStock stock, double price, int count);

        public MarketClientInfo GetClientInfo();
    }

    public class MarketClientInfo
    {
        public string Name { get; init; }
        public ClientApiName ApiName { get; init; }
        public StockClientReserve[] Reserves { get; init; }

        public MarketClientInfo(string name, ClientApiName apiName, StockClientReserve[] clientReserve)
        {
            Name = name;
            ApiName = apiName;
            Reserves = clientReserve;
        }
    }

    public enum ClientApiName
    {
        Tinkkoff,
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
