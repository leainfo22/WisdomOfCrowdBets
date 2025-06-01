using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomOfCrowndBets.Core.DTO.Api;
using WisdomOfCrowndBets.Core.DTO;

namespace WisdomOfCrowndBets.Core.Interfaces.Analysis
{
    public interface IAverageOdds
    {
        public Task CalculateAvaregeOdds(List<EventDTO> listEvent);

    }
}
