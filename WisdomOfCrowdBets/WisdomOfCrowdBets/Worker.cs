using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WisdomOfCrowndBets.Core.DTO;
using WisdomOfCrowndBets.Core.DTO.Api;
using WisdomOfCrowndBets.Core.DTO.Team;
using WisdomOfCrowndBets.Core.Interfaces;
using WisdomOfCrowndBets.Core.Interfaces.Analysis;


namespace WisdomOfCrowdBets
{
    public class Worker : BackgroundService
    {
        private readonly ApiOddsConfig _apiGetOdds;
        private readonly Xlsx _xlsx;
        private readonly AFLFile _file;
        private readonly Bets _bets;
        private readonly Email _email;
        private readonly IConfiguration _configuration;
        private readonly IGetApiData _getApiData;
        private readonly IGetXlsxHistoricalData _getXlsxHistoricalData;
        private readonly IEventAnalysis _eventAnalysis;
        private readonly IHistoricalTeamStatistics _historicalTeamStatistics;
        private readonly IMatchNames _matchNames;


        public Worker(  IConfiguration configuration, 
                        IGetApiData getApiData, 
                        IGetXlsxHistoricalData getXlsxHistoricalData,
                        IEventAnalysis eventAnalysis,
                        IHistoricalTeamStatistics historicalTeamStatistics,
                        IMatchNames matchNames)
        {
            _configuration = configuration;
            _getApiData = getApiData;
            _getXlsxHistoricalData = getXlsxHistoricalData;
            _eventAnalysis = eventAnalysis;
            _historicalTeamStatistics = historicalTeamStatistics;
            _matchNames = matchNames;

            _apiGetOdds = _configuration.GetSection("ApiGetOdds").Get<ApiOddsConfig>();
            _xlsx = _configuration.GetSection("Xlsx").Get<Xlsx>();
            _file = _configuration.GetSection("File").Get<AFLFile>();
            _bets = _configuration.GetSection("Bets").Get<Bets>();
            _email = _configuration.GetSection("Email").Get<Email>();

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {   
                //Get data from the API
                List<EventDTO> listEvent = await _getApiData.GetData(_apiGetOdds);
                //Get data from the XLSX file that was downloaded in
                List<HistoricalDataXlsx> xlsxData = await _getXlsxHistoricalData.GetExelData(_xlsx);
                //Normalize team names in the XLSX file and match them with the API data
                await _matchNames.MatchTeamsNames(_file.AFLFilePath, listEvent);
                //Calculate average odds for each outcome - Home and Away (No draw in AFL API GET)
                //And convert average odds to implied probabilities
                await _eventAnalysis.CalculateAvaregeOdds(listEvent);
                List<TeamStatistic> teamStatistic = await _historicalTeamStatistics.CalculateHistoricalTeamStatistics(xlsxData);
                //Calculate home and away win rates based on historical data to compare with implied probabilities
                await _eventAnalysis.CalculateHomeAwayWinrate(listEvent, teamStatistic);
                //Simulate betting on the outcome with the lowest average odds (10,50,100,500,1000 AUD)
                //%5 fee is applied to the profit -  (Betfair -  most sport )
                await _eventAnalysis.EstimateProfit(listEvent, _bets);
                //Notify if the average odds imply a higher probability than a single bookmaker’s odds
                await _eventAnalysis.ValuableBetNotification(listEvent, _email);
                // Delay to avoid continuous execution - It could be adjusted based on the API rate limits or requirements
                //or depending on how often you want to run the analysis
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken); 
            
            }
        }
    }
}
