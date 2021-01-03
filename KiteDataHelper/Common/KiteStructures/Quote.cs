using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct Quote
    {
        public Quote(Dictionary<string, dynamic> data)
        {
            try
            {
                InstrumentToken = Convert.ToUInt32(data["instrument_token"]);
                Timestamp = CommonFunctions.StringToDate(data["timestamp"]);
                LastPrice = data["last_price"];

                Change = data["net_change"];

                Open = data["ohlc"]["open"];
                Close = data["ohlc"]["close"];
                Low = data["ohlc"]["low"];
                High = data["ohlc"]["high"];

                if (data.ContainsKey("last_quantity"))
                {
                    // Non index quote
                    LastQuantity = Convert.ToUInt32(data["last_quantity"]);
                    LastTradeTime = CommonFunctions.StringToDate(data["last_trade_time"]);
                    AveragePrice = data["average_price"];
                    Volume = Convert.ToUInt32(data["volume"]);

                    BuyQuantity = Convert.ToUInt32(data["buy_quantity"]);
                    SellQuantity = Convert.ToUInt32(data["sell_quantity"]);

                    OI = Convert.ToUInt32(data["oi"]);

                    OIDayHigh = Convert.ToUInt32(data["oi_day_high"]);
                    OIDayLow = Convert.ToUInt32(data["oi_day_low"]);

                    LowerCircuitLimit = data["lower_circuit_limit"];
                    UpperCircuitLimit = data["upper_circuit_limit"];

                    Bids = new List<DepthItem>();
                    Offers = new List<DepthItem>();

                    if (data["depth"]["buy"] != null)
                    {
                        foreach (Dictionary<string, dynamic> bid in data["depth"]["buy"])
                            Bids.Add(new DepthItem(bid));
                    }

                    if (data["depth"]["sell"] != null)
                    {
                        foreach (Dictionary<string, dynamic> offer in data["depth"]["sell"])
                            Offers.Add(new DepthItem(offer));
                    }
                }
                else
                {
                    // Index quote
                    LastQuantity = 0;
                    LastTradeTime = null;
                    AveragePrice = 0;
                    Volume = 0;

                    BuyQuantity = 0;
                    SellQuantity = 0;

                    OI = 0;

                    OIDayHigh = 0;
                    OIDayLow = 0;

                    LowerCircuitLimit = 0;
                    UpperCircuitLimit = 0;

                    Bids = new List<DepthItem>();
                    Offers = new List<DepthItem>();
                }
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }

        }

        public UInt32 InstrumentToken { get; set; }
        public decimal LastPrice { get; set; }
        public UInt32 LastQuantity { get; set; }
        public decimal AveragePrice { get; set; }
        public UInt32 Volume { get; set; }
        public UInt32 BuyQuantity { get; set; }
        public UInt32 SellQuantity { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Change { get; set; }
        public decimal LowerCircuitLimit { get; set; }
        public decimal UpperCircuitLimit { get; set; }
        public List<DepthItem> Bids { get; set; }
        public List<DepthItem> Offers { get; set; }

        // KiteConnect 3 Fields

        public DateTime? LastTradeTime { get; set; }
        public UInt32 OI { get; set; }
        public UInt32 OIDayHigh { get; set; }
        public UInt32 OIDayLow { get; set; }
        public DateTime? Timestamp { get; set; }
    }
}
