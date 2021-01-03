using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct Holding
    {
        public Holding(Dictionary<string, dynamic> data)
        {
            try
            {
                Product = data["product"];
                Exchange = data["exchange"];
                Price = data["price"];
                LastPrice = data["last_price"];
                CollateralQuantity = Convert.ToInt32(data["collateral_quantity"]);
                PNL = data["pnl"];
                ClosePrice = data["close_price"];
                AveragePrice = data["average_price"];
                TradingSymbol = data["tradingsymbol"];
                CollateralType = data["collateral_type"];
                T1Quantity = Convert.ToInt32(data["t1_quantity"]);
                InstrumentToken = Convert.ToUInt32(data["instrument_token"]);
                ISIN = data["isin"];
                RealisedQuantity = Convert.ToInt32(data["realised_quantity"]);
                Quantity = Convert.ToInt32(data["quantity"]);
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }

        public string Product { get; set; }
        public string Exchange { get; set; }
        public decimal Price { get; set; }
        public decimal LastPrice { get; set; }
        public int CollateralQuantity { get; set; }
        public decimal PNL { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal AveragePrice { get; set; }
        public string TradingSymbol { get; set; }
        public string CollateralType { get; set; }
        public int T1Quantity { get; set; }
        public UInt32 InstrumentToken { get; set; }
        public string ISIN { get; set; }
        public int RealisedQuantity { get; set; }
        public int Quantity { get; set; }
    }
}
