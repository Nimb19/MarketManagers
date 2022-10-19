namespace CryptoMarketManager.Core.Models;

public class BinanceApiClientOptions
{
    //public string AppName { get; set; }
    //public string GrpcClientName { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
    public bool IsSandboxMode { get; set; }
}
