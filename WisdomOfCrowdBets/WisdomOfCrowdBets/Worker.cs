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
        private readonly AFLFile _file;
        private readonly IConfiguration _configuration;
        private readonly IGetApiData _getApiData;
        private readonly IGetXlsxHistoricalData _getXlsxHistoricalData;
        private readonly IEventAnalysis _averageOdds;
        private readonly IHistoricalTeamStatistics _historicalTeamStatistics;
        private readonly IMatchNames _matchNames;


        public Worker(  IConfiguration configuration, 
                        IGetApiData getApiData, 
                        IGetXlsxHistoricalData getXlsxHistoricalData,
                        IEventAnalysis averageOdds,
                        IHistoricalTeamStatistics historicalTeamStatistics,
                        IMatchNames matchNames)
        {
            _configuration = configuration;
            _getApiData = getApiData;
            _getXlsxHistoricalData = getXlsxHistoricalData;
            _averageOdds = averageOdds;
            _historicalTeamStatistics = historicalTeamStatistics;
            _matchNames = matchNames;

            _apiGetOdds = _configuration.GetSection("ApiGetOdds").Get<ApiOddsConfig>();
            _xlsx = _configuration.GetSection("Xlsx").Get<Xlsx>();
            _file = _configuration.GetSection("File").Get<AFLFile>();


        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {       
                List<EventDTO> listEvent = await _getApiData.GetData(_apiGetOdds);
                List<HistoricalDataXlsx> xlsxData = await _getXlsxHistoricalData.GetExelData(_xlsx);
                await _matchNames.MatchTeamsNames(_file.AFLFilePath, listEvent);
                await _averageOdds.CalculateAvaregeOdds(listEvent);
                List<TeamStatistic> teamStatistic = await _historicalTeamStatistics.CalculateHistoricalTeamStatistics(xlsxData);
                await _averageOdds.CalculateHomeAwayWinrate(listEvent, teamStatistic);
            }

        }
    }
}
