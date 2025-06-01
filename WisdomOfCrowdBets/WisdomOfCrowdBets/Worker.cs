using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using WisdomOfCrowndBets.Core.DTO;
using WisdomOfCrowndBets.Core.DTO.Api;
using WisdomOfCrowndBets.Core.DTO.Team;
using WisdomOfCrowndBets.Core.Interfaces;
using WisdomOfCrowndBets.Core.Interfaces.Analysis;
using WisdomOfCrowndBets.Core.Services;
using WisdomOfCrowndBets.Core.Services.Analysis;

namespace WisdomOfCrowdBets
{
    public class Worker : BackgroundService
    {
        private readonly ApiOddsConfig _apiGetOdds;
        private readonly Xlsx _xlsx;
        private readonly IConfiguration _configuration;
        private readonly IGetApiData _getApiData;
        private readonly IGetXlsxHistoricalData _getXlsxHistoricalData;

        private readonly IAverageOdds _averageOdds;
        private readonly IHistoricalTeamStatistics _historicalTeamStatistics;

        public Worker(  IConfiguration configuration, 
                        IGetApiData getApiData, 
                        IGetXlsxHistoricalData getXlsxHistoricalData,
                        IAverageOdds averageOdds,
                        IHistoricalTeamStatistics historicalTeamStatistics)
        {
            _configuration = configuration;
            _getApiData = getApiData;
            _getXlsxHistoricalData = getXlsxHistoricalData;
            _averageOdds = averageOdds;
            _historicalTeamStatistics = historicalTeamStatistics;
            _apiGetOdds = _configuration.GetSection("ApiGetOdds").Get<ApiOddsConfig>();
            _xlsx = _configuration.GetSection("Xlsx").Get<Xlsx>();


        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {       
                List<EventDTO> listEvent = await _getApiData.GetData(_apiGetOdds);
                var xlsxData = await _getXlsxHistoricalData.GetExelData(_xlsx);
                await _averageOdds.CalculateAvaregeOdds(listEvent);
                List<TeamStatistic> teamStatistic = await _historicalTeamStatistics.CalculateHistoricalTeamStatistics(xlsxData);

            }

        }
    }
}
