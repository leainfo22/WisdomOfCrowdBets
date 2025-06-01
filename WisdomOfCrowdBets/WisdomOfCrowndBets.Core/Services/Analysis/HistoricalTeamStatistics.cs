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
                // Get all unique team names
                var allTeams = listHistoricalDataXlsx
                    .SelectMany(x => new[] { x.HomeTeam, x.AwayTeam })
                    .Where(t => !string.IsNullOrEmpty(t))
                    .Distinct();

                foreach (var team in allTeams)
                {
                    // Matches played as home or away
                    int matchesPlayed = listHistoricalDataXlsx.Count(x => x.HomeTeam == team || x.AwayTeam == team);

                    // Wins as home
                    int homeWins = listHistoricalDataXlsx.Count(x => x.HomeTeam == team && x.Winner == "Home");
                    // Wins as away
                    int awayWins = listHistoricalDataXlsx.Count(x => x.AwayTeam == team && x.Winner == "Away");
                    int totalWins = homeWins + awayWins;

                    // Losses: played - wins
                    int losses = matchesPlayed - totalWins;

                    teamStatistics.Add(new TeamStatistic
                    {
                        team_name = team,
                        matches_played = matchesPlayed,
                        wins = totalWins,
                        losses = losses,
                        home_wins = homeWins,
                        away_wins = awayWins
                    });

                    Console.WriteLine($"Team: {team}, Played: {matchesPlayed}, Wins: {totalWins}, Losses: {losses}, Home Wins: {homeWins}, Away Wins: {awayWins}");
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
