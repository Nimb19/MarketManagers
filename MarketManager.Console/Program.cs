using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddInvestApiClient((_, settings) =>
        { 
            context.Configuration.Bind(settings);
        });
    })
    .Build()
    .RunAsync();