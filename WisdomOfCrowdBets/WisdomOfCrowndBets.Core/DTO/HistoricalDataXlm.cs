using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomOfCrowndBets.Core.DTO
{
    public class HistoricalDataXlm
    {
        public DateTime Date { get; set; }
        public string? HomeTeam { get; set; }
        public string? AwayTeam { get; set; }
        public decimal HomeOdds { get; set; }
        public decimal AwayOdds { get; set; }
    }
}
