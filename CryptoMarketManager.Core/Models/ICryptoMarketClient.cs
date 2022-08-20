namespace CryptoMarketManager.Core.Models
{
    public interface ICryptoMarketClient : IDisposable
    {
        public CryptoCoinFrameStatistic[] GetStatistic(ICryptoCoin coin, TimeSpan interval, DateTimeOffset dateFrom, DateTimeOffset dateTo);
        public bool Buy(ICryptoCoin coin, double price, double count);
        public bool Sell(ICryptoCoin coin, double price, double count);

        public CryptoClientInfo GetClientInfo();
    }

    public class CryptoClientInfo
    {
        public class CryptoCoinClientReserve
        {

        }
        public CryptoCoinClientReserve[] ClientReserve { get; }
    }

    public class CryptoCoinFrameStatistic
    {
        public ICryptoCoin Coin { get; }
        public double Price { get; }
    }
}
