using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct Instrument
    {
        public Instrument(Dictionary<string, dynamic> data)
        {
            try
            {
                InstrumentToken = Convert.ToUInt32(data["instrument_token"]);
                ExchangeToken = Convert.ToUInt32(data["exchange_token"]);
                TradingSymbol = data["tradingsymbol"];
                Name = data["name"];
                LastPrice = CommonFunctions.StringToDecimal(data["last_price"]);
                TickSize = CommonFunctions.StringToDecimal(data["tick_size"]);
                Expiry = CommonFunctions.StringToDate(data["expiry"]);
                InstrumentType = data["instrument_type"];
                Segment = data["segment"];
                Exchange = data["exchange"];
                Strike = CommonFunctions.StringToDecimal(data["strike"]);

                LotSize = Convert.ToUInt32(data["lot_size"]);
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }

        }

        public UInt32 InstrumentToken { get; set; }
        public UInt32 ExchangeToken { get; set; }
        public string TradingSymbol { get; set; }
        public string Name { get; set; }
        public decimal LastPrice { get; set; }
        public decimal TickSize { get; set; }
        public DateTime? Expiry { get; set; }
        public string InstrumentType { get; set; }
        public string Segment { get; set; }
        public string Exchange { get; set; }
        public decimal Strike { get; set; }
        public UInt32 LotSize { get; set; }
    }
}
