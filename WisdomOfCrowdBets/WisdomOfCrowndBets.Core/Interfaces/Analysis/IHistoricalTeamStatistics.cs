using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomOfCrowndBets.Core.DTO;
using WisdomOfCrowndBets.Core.DTO.Api;
using WisdomOfCrowndBets.Core.DTO.Team;

namespace WisdomOfCrowndBets.Core.Interfaces.Analysis
{
    public interface IHistoricalTeamStatistics
    {
        public Task<List<TeamStatistic>> CalculateHistoricalTeamStatistics(List<HistoricalDataXlsx> listHistoricalDataXlsx);

    }
}
