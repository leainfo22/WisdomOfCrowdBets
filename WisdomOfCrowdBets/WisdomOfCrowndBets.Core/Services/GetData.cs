using WisdomOfCrowndBets.Core.DTO;
using WisdomOfCrowndBets.Core.Interfaces;

namespace WisdomOfCrowndBets.Core.Services
{
    public class GetData : IGetData
    {
        private readonly IOddsApi _oddsApi;

        public GetData(IOddsApi oddsApi)
        {
            _oddsApi = oddsApi;          
        }
        public async Task GetApiData(ApiOddsConfig apiGetOdds) 
        {
            var response = await _oddsApi.GetOddsAsync(apiGetOdds);
        }

    }
}
