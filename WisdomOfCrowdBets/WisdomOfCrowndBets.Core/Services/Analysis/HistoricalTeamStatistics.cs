using System;
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
    public class HistoricalTeamStatistics : IHistoricalTeamStatistics
    {
        public async Task<List<TeamStatistic>> CalculateHistoricalTeamStatistics(List<HistoricalDataXlsx> listHistoricalDataXlsx) 
        {
            List<TeamStatistic> teamStatistics = new List<TeamStatistic>();
            try
            {
                var teamStats = listHistoricalDataXlsx
                    .SelectMany(x => new[]
                    {
                        new { Team = x.HomeTeam, IsHome = true, IsWinner = x.Winner == x.HomeTeam },
                        new { Team = x.AwayTeam, IsHome = false, IsWinner = x.Winner == x.AwayTeam }
                    })
                    .GroupBy(x => x.Team)
                    .Select(g => new
                    {
                        Team = g.Key,
                        HomeWins = g.Count(x => x.IsHome && x.IsWinner),
                        AwayWins = g.Count(x => !x.IsHome && x.IsWinner)
                    })
                    .ToList();

                foreach (var stat in teamStats)
                {
                    Console.WriteLine($"Team: {stat.Team}, Home Wins: {stat.HomeWins}, Away Wins: {stat.AwayWins}");
                }
                return teamStatistics;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return teamStatistics;
            }
        }
    }
}
