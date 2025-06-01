using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomOfCrowndBets.Core.DTO.Api
{
    public class EventDTO
    {
        public string id { get; set; }
        public string sport_key { get; set; }
        public string sport_title { get; set; }
        public DateTime commence_time { get; set; }
        public string home_team { get; set; }
        public string away_team { get; set; }
        public List<Bookmaker> bookmakers { get; set; }
    }
}
