using KiteDataHelper.Common.Enums;
using KiteDataHelper.Common.KiteStructures;
using KiteDataHelper.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KiteDataHelper.Common.Interfaces.Services
{
    public interface IMarketDataAccessService
    {
        Task<List<Instrument>> GetInstrumentsList();
        Task<List<KiteCandleUnit>> GetData(uint instrumentToken, Interval granularity, DateTime startDate, DateTime endDate);
        Task<decimal> GetLtp(string query);
    }
}
