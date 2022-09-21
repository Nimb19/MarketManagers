using MarketManager.Core.Models.MarketClients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tinkoff.InvestApi;
using Tinkoff.InvestApi.V1;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((app) =>
    {
        app.AddJsonFile("appsettings.json", true);
        app.AddJsonFile("appsettings.Development.json", true);
        app.AddJsonFile("appsettings.Production.json", true);
    })
    .ConfigureServices((context, services) =>
    {
        services.AddInvestApiClient((_, settings) =>
        {
            context.Configuration.Bind(settings);
            settings.AppName = AppDomain.CurrentDomain.FriendlyName;
        });
        services.AddSingleton<TinkoffInvestApiClient>();
    })
    .Build();

using (var scope = host.Services.CreateScope())
{
    var tincoffApi = scope.ServiceProvider.GetRequiredService<InvestApiClient>();

    var accounts = tincoffApi.Users.GetAccounts();
    var accountId = accounts.Accounts.First().Id;

    var oper = tincoffApi.Operations.GetOperations(new OperationsRequest()
    {
        AccountId = accountId,
    });
}

host.RunAsync();