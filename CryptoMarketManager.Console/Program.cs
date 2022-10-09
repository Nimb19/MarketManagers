using Binance.Net;
using CommonTools.HostBuilderExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Host.CreateDefaultBuilder(args)
    .GymDefaultConfigure()
    .ConfigureServices((context, services) =>
    {
        //services.AddBinance((_, settings) =>
        //{
        //    context.Configuration.Bind(settings);
        //    settings.AppName = AppDomain.CurrentDomain.FriendlyName;
        //});
        //services.AddHostedService<MainService>();
    })
    .Build()
    .WriteInitializeMessage()
    .Run();