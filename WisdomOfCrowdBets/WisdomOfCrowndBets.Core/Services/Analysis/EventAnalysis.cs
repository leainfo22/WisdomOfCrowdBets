using MathNet.Numerics.Distributions;
using NPOI.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomOfCrowndBets.Core.DTO;
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
                var teamHomeStat = listTeams.FirstOrDefault(t => t.team_name == ev.home_team);
                ev.probability_home_win = teamHomeStat?.WinRate;
                var teamAwayStat = listTeams.FirstOrDefault(t => t.team_name == ev.away_team);
                ev.probability_home_win = teamAwayStat?.WinRate;
            }
        }

        public async Task EstimateProfit(List<EventDTO> listEvent, Bets bets) 
        {
            float fee = float.Parse(bets.Fee);
            string[] amountsArray = bets.Amounts.Split(',');
            foreach (var ev in listEvent) 
            {
                float least_avg_odd = 0;
                if (ev.avg_away_odd < ev.avg_home_odd)                
                    least_avg_odd = (float)ev.avg_away_odd;                
                else
                    least_avg_odd = (float)ev.avg_home_odd;
                Console.WriteLine($"Match: {ev.home_team} v/s {ev.away_team} ");
                foreach (string am in amountsArray)
                {
                    if (int.TryParse(am.Trim(), out int number))
                    {
                        var grossProfit = (least_avg_odd * number) - number;
                        // Calculate the house fee amount
                        var houseFeeAmount = (fee / 100) * grossProfit;
                        // Calculate the net profit
                        var netProfit = grossProfit - houseFeeAmount;
                        Console.WriteLine($"Amount bet: {number} Profit: {netProfit}");
                    }                    
                }                
            }
        }
    }
}
