using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using WisdomOfCrowndBets.Core.DTO;
using WisdomOfCrowndBets.Core.DTO.Api;
using WisdomOfCrowndBets.Core.Interfaces;
using WisdomOfCrowndBets.Core.Services;

namespace WisdomOfCrowdBets
{
    public class Worker : BackgroundService
    {
        private readonly ApiOddsConfig _apiGetOdds;
        private readonly Xlsx _xlsx;
        private readonly IConfiguration _configuration;
        private readonly IGetApiData _getApiData;
        private readonly IGetXlsxHistoricalData _getXlsxHistoricalData;

        public Worker(IConfiguration configuration, IGetApiData getApiData, IGetXlsxHistoricalData getXlsxHistoricalData)
        {
            _configuration = configuration;
            _getApiData = getApiData;
            _getXlsxHistoricalData = getXlsxHistoricalData;
            _apiGetOdds = _configuration.GetSection("ApiGetOdds").Get<ApiOddsConfig>();
            _xlsx = _configuration.GetSection("Xlsx").Get<Xlsx>();

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var xlsxData = await _getXlsxHistoricalData.GetExelData(_xlsx);
                    List<EventDTO> listEvent = await _getApiData.GetData(_apiGetOdds);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
    }
}
