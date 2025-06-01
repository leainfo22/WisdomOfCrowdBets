using System.Collections.Generic;
using WisdomOfCrowndBets.Core.DTO;
using WisdomOfCrowndBets.Core.DTO.Api;
using WisdomOfCrowndBets.Core.Interfaces;

namespace WisdomOfCrowndBets.Core.Services
{
    public class GetApiData : IGetApiData
    {
        private readonly IGetOdds _oddsApi;

        public GetApiData(IGetOdds oddsApi)
        {
            _oddsApi = oddsApi;          
        }
        public async Task<List<EventDTO>> GetData(ApiOddsConfig apiGetOdds) 
        {
            return await _oddsApi.GetOddsAsync(apiGetOdds);
        }

    }
}
