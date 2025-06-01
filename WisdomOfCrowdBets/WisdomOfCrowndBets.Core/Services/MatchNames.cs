using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomOfCrowndBets.Core.DTO.Api;
using WisdomOfCrowndBets.Core.Interfaces;

namespace WisdomOfCrowndBets.Core.Services
{
    public class MatchNames : IMatchNames
    {
        public async Task MatchTeamsNames(string filePath, List<EventDTO> listEvent) 
        {
            var replacements = File.ReadAllLines(filePath)
                                        .Where(line => !string.IsNullOrWhiteSpace(line) && line.Contains(','))
                                        .Select(line => line.Split(',', 2))
                                        .ToDictionary(parts => parts[0].Trim(), parts => parts[1].Trim());

            foreach (var ev in listEvent)
            {
                if (ev.home_team != null && replacements.TryGetValue(ev.home_team, out var newName1))
                {
                    ev.home_team = newName1;
                }
                if (ev.away_team != null && replacements.TryGetValue(ev.away_team, out var newName2))
                {
                    ev.away_team = newName2;
                }
                if (ev.bookmakers == null) continue;
                foreach (var bookmaker in ev.bookmakers)
                {
                    if (bookmaker.markets == null) continue;
                    foreach (var market in bookmaker.markets)
                    {
                        if (market.outcomes == null) continue;
                        foreach (var outcome in market.outcomes)
                        {
                            if (outcome.name != null && replacements.TryGetValue(outcome.name, out var newName3))
                            {
                                outcome.name = newName3;
                            }
                        }
                    }
                }
            }
        }

    }
}
