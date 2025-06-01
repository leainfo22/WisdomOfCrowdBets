using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WisdomOfCrowdBets;
using WisdomOfCrowndBets.Core.Interfaces;
using WisdomOfCrowndBets.Core.Services;
using WisdomOfCrowndBets.Infrastructure.Repositories;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IOddsApi, OddsApi>();
        services.AddSingleton<IGetData, GetData>();
        services.AddHostedService<Worker>();

    }).Build();

await host.RunAsync();

