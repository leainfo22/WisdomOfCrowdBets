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

            foreach (var evento in listEvent)
            {
                if (evento.bookmakers == null) continue;
                foreach (var bookmaker in evento.bookmakers)
                {
                    if (bookmaker.markets == null) continue;
                    foreach (var market in bookmaker.markets)
                    {
                        if (market.outcomes == null) continue;
                        foreach (var outcome in market.outcomes)
                        {
                            if (outcome.name != null && replacements.TryGetValue(outcome.name, out var newName))
                            {
                                outcome.name = newName;
                            }
                        }
                    }
                }
            }
        }

    }
}
