using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct GTTCondition
    {
        public GTTCondition(Dictionary<string, dynamic> data)
        {
            try
            {
                InstrumentToken = 0;
                if (data.ContainsKey("instrument_token"))
                {
                    InstrumentToken = Convert.ToUInt32(data["instrument_token"]);
                }
                Exchange = data["exchange"];
                TradingSymbol = data["tradingsymbol"];
                TriggerValues = (data["trigger_values"] as ArrayList).Cast<decimal>().ToList();
                LastPrice = data["last_price"];
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }

        public UInt32 InstrumentToken { get; set; }
        public string Exchange { get; set; }
        public string TradingSymbol { get; set; }
        public List<decimal> TriggerValues { get; set; }
        public decimal LastPrice { get; set; }
    }
}
