namespace MarketManager.Core.Models;

public class TinkoffInvestApiClientOptions
{
    public string AppName { get; set; }
    public string GrpcClientName { get; set; }
    public string AccessToken { get; set; }
    public bool IsSandboxMode { get; set; }
}
