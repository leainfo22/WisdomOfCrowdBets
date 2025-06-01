using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomOfCrowndBets.Core.DTO
{
    public class ApiOddsConfig
    {
        public string? ApiGetOddsUrl { get; set; }
        public string? ApiGetOddsSport { get; set; }
        public string? ApiGetOddsHeaderApiKey { get; set; }
        public string? ApiGetOddsApiKey { get; set; }
        public string? ApiGetOddsHeaderRegion { get; set; }
        public string? ApiGetOddsRegion { get; set; }
        public string? ApiGetOddsHeaderMarkets { get; set; }
        public string? ApiGetOddsMarkets { get; set; }
    }
}
