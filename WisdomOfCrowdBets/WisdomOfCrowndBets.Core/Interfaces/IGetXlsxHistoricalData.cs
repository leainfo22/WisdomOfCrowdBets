using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WisdomOfCrowndBets.Core.DTO;

namespace WisdomOfCrowndBets.Core.Interfaces
{
    public interface IGetXlsxHistoricalData
    {
        public Task<List<HistoricalDataXlsx>> GetExelData(Xlsx xml);

    }
}
