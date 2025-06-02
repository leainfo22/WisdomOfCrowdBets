using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WisdomOfCrowdBets;
using WisdomOfCrowndBets.Core.Interfaces;
using WisdomOfCrowndBets.Core.Interfaces.Analysis;
using WisdomOfCrowndBets.Core.Services;
using WisdomOfCrowndBets.Core.Services.Analysis;
using WisdomOfCrowndBets.Infrastructure.Repositories;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IGetOdds, GetOdds>();
        services.AddSingleton<IGetApiData, GetApiData>();
        services.AddSingleton<IGetXlsxHistoricalData, GetXmlHistoricalData>();
        services.AddSingleton<IEventAnalysis, EventAnalysis>();
        services.AddSingleton<IHistoricalTeamStatistics,HistoricalTeamStatistics>();
        services.AddSingleton<IMatchNames, MatchNames>();
        services.AddSingleton<ISendEmail, SendEmail>();

        services.AddHostedService<Worker>();

    }).Build();

await host.RunAsync();

