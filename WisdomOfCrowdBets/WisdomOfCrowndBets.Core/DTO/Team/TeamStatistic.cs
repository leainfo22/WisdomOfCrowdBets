using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomOfCrowndBets.Core.DTO.Team
{
    public class TeamStatistic
    {
        public string? team_name { get; set; }
        public int matches_played { get; set; }
        public int wins { get; set; }
        public int losses { get; set; }
        public int home_wins { get; set; }
        public int away_wins { get; set; }

    }
}
