using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WisdomOfCrowndBets.Core.DTO.Api
{
    public class Market
    {
        public string key { get; set; }
        public DateTime last_update { get; set; }
        public List<Outcome> outcomes { get; set; }
    }
}
