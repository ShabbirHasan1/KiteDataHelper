using System;

namespace Models
{
    public class KiteDataRequest
    {
        public string TradingSymbol { get; set; }
        public string Interval { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
