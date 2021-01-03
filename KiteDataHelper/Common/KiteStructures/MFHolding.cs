using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct MFHolding
    {
        public MFHolding(Dictionary<string, dynamic> data)
        {
            try
            {
                Quantity = data["quantity"];
                Fund = data["fund"];
                Folio = data["folio"];
                AveragePrice = data["average_price"];
                TradingSymbol = data["tradingsymbol"];
                LastPrice = data["last_price"];
                PNL = data["pnl"];
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }

        public decimal Quantity { get; }
        public string Fund { get; }
        public string Folio { get; }
        public decimal AveragePrice { get; }
        public string TradingSymbol { get; }
        public decimal LastPrice { get; }
        public decimal PNL { get; }
    }
}
