using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomOfCrowndBets.Core.DTO;
using WisdomOfCrowndBets.Core.DTO.Api;

namespace WisdomOfCrowndBets.Core.Interfaces
{
    public interface IGetApiData
    {
        public Task<List<EventDTO>> GetData(ApiOddsConfig apiGetOdds);


    }
}
