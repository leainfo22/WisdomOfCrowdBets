using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomOfCrowndBets.Core.DTO.Api;
using WisdomOfCrowndBets.Core.DTO.Team;
using WisdomOfCrowndBets.Core.Interfaces.Analysis;

namespace WisdomOfCrowndBets.Core.Services.Analysis
{
    public class EventAnalysis : IEventAnalysis
    {
        public async Task CalculateAvaregeOdds(List<EventDTO> listEvent) 
        {
            try
            {
                foreach (var ev in listEvent)
                {
                    var homePrices = ev.bookmakers?
                        .SelectMany(b => b.markets ?? new List<Market>())
                        .SelectMany(m => m.outcomes ?? new List<Outcome>())
                        .Where(o => o.name == ev.home_team)
                        .Select(o => o.price)
                        .ToList();

                    var awayPrices = ev.bookmakers?
                        .SelectMany(b => b.markets ?? new List<Market>())
                        .SelectMany(m => m.outcomes ?? new List<Outcome>())
                        .Where(o => o.name == ev.away_team)
                        .Select(o => o.price)
                        .ToList();

                    ev.avg_home_odd = (homePrices != null && homePrices.Any()) ? (float?)homePrices.Average() : null;
                    ev.avg_away_odd = (awayPrices != null && awayPrices.Any()) ? (float?)awayPrices.Average() : null;
                    ev.avg_away_odd_implied_probability = ev.avg_away_odd.HasValue ? 1 / ev.avg_away_odd.Value : null;
                    ev.avg_home_odd_implied_probability = ev.avg_home_odd.HasValue ? 1 / ev.avg_home_odd.Value : null;
                    Console.WriteLine(homePrices);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task CalculateHomeAwayWinrate(List<EventDTO> listEvent, List<TeamStatistic> listTeams) 
        {
            foreach (var ev in listEvent)
            {
                var teamStat = listTeams.FirstOrDefault(t => t.team_name == ev.home_team);
                ev.probability_home_win = teamStat?.WinRate;
            }
        }

    }
}
