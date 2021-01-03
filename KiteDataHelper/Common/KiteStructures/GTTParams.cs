using System;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct GTTParams
    {
        public string TradingSymbol { get; set; }
        public string Exchange { get; set; }
        public UInt32 InstrumentToken { get; set; }
        public string TriggerType { get; set; }
        public decimal LastPrice { get; set; }
        public List<GTTOrderParams> Orders { get; set; }
        public List<decimal> TriggerPrices { get; set; }
    }
}
