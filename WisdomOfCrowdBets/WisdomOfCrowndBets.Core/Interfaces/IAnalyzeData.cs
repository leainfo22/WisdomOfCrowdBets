using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomOfCrowndBets.Core.DTO;

namespace WisdomOfCrowndBets.Core.Interfaces
{
    public interface IAnalyzeData
    {
        public Task GetExelData(ApiOddsConfig apiGetOdds);

    }
}
