using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using WisdomOfCrowndBets.Core.DTO;
using WisdomOfCrowndBets.Core.Interfaces;

namespace WisdomOfCrowdBets
{
    public class Worker : BackgroundService
    {
        private readonly ApiOddsConfig _apiGetOdds;
        private readonly Xml _xml;
        private readonly IConfiguration _configuration;
        private readonly IGetData _getData;
        public Worker(IConfiguration configuration, IGetData getData)
        {
            _configuration = configuration;
            _getData = getData;
            _apiGetOdds = _configuration.GetSection("ApiGetOdds").Get<ApiOddsConfig>();
            _xml = _configuration.GetSection("XmlPath").Get<Xml>();

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _getData.GetApiData(_apiGetOdds);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

        }
    }
}
