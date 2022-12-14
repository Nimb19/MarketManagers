namespace CryptoMarketManager.Core.Models;

public interface ICryptoMarketClient : IDisposable
{
    /// <summary>
    ///     Инициализация соединения, взятие инфы об аккаунте, если sandbox выдача баланса, вывод инфы в лог об инициализации или ошибке.
    /// </summary>
    public Task<bool> Init(CancellationToken ct);
    public CryptoCoinFrameStatistic[] GetStatistic(ICryptoCoin coin, TimeSpan interval, DateTimeOffset dateFrom, DateTimeOffset dateTo, CancellationToken ct);
    public bool Buy(ICryptoCoin coin, double price, double count, CancellationToken ct);
    public bool Sell(ICryptoCoin coin, double price, double count, CancellationToken ct);

    public CryptoClientInfo GetClientInfo();
}

public class CryptoClientInfo
{
    public string Name { get; init; }
    public CryptoClientApiName ApiName { get; init; }
    public CryptoCoinClientReserve[] Reserves { get; init; }

    public CryptoClientInfo(string name, CryptoClientApiName apiName, CryptoCoinClientReserve[] clientReserve)
    {
        Name = name;
        ApiName = apiName;
        Reserves = clientReserve;
    }
}

public enum CryptoClientApiName
{
    Binance,
    OKX,
    CryptoCom
}

public class CryptoCoinClientReserve
{
    public ICryptoCoin Coin { get; init; }
    public double Reserve { get; init; }

    public CryptoCoinClientReserve(ICryptoCoin coin, double reserve)
    {
        Coin = coin;
        Reserve = reserve;
    }
}

public class CryptoCoinFrameStatistic
{
    public ICryptoCoin Coin { get; init; }
    public DateTimeOffset FrameDate { get; init; }
    public double Price { get; init; }
}
