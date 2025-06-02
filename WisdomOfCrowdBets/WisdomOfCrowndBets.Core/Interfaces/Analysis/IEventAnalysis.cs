using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomOfCrowndBets.Core.DTO.Api;
using WisdomOfCrowndBets.Core.DTO;
using WisdomOfCrowndBets.Core.DTO.Team;

namespace WisdomOfCrowndBets.Core.Interfaces.Analysis
{
    public interface IEventAnalysis
    {
        public Task CalculateAvaregeOdds(List<EventDTO> listEvent);
        public Task CalculateHomeAwayWinrate(List<EventDTO> listEvent,List<TeamStatistic> listTeams);
        public Task EstimateProfit(List<EventDTO> listEvent, Bets bets);

    }
}
