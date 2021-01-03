using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KiteDataHelper.Common.KiteExceptions;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct Position
    {
        public Position(Dictionary<string, dynamic> data)
        {
            try
            {
                Product = data["product"];
                OvernightQuantity = Convert.ToInt32(data["overnight_quantity"]);
                Exchange = data["exchange"];
                SellValue = data["sell_value"];
                BuyM2M = data["buy_m2m"];
                LastPrice = data["last_price"];
                TradingSymbol = data["tradingsymbol"];
                Realised = data["realised"];
                PNL = data["pnl"];
                Multiplier = data["multiplier"];
                SellQuantity = Convert.ToInt32(data["sell_quantity"]);
                SellM2M = data["sell_m2m"];
                BuyValue = data["buy_value"];
                BuyQuantity = Convert.ToInt32(data["buy_quantity"]);
                AveragePrice = data["average_price"];
                Unrealised = data["unrealised"];
                Value = data["value"];
                BuyPrice = data["buy_price"];
                SellPrice = data["sell_price"];
                M2M = data["m2m"];
                InstrumentToken = Convert.ToUInt32(data["instrument_token"]);
                ClosePrice = data["close_price"];
                Quantity = Convert.ToInt32(data["quantity"]);
                DayBuyQuantity = Convert.ToInt32(data["day_buy_quantity"]);
                DayBuyValue = data["day_buy_value"];
                DayBuyPrice = data["day_buy_price"];
                DaySellQuantity = Convert.ToInt32(data["day_sell_quantity"]);
                DaySellValue = data["day_sell_value"];
                DaySellPrice = data["day_sell_price"];
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }

        }

        public string Product { get; }
        public int OvernightQuantity { get; }
        public string Exchange { get; }
        public decimal SellValue { get; }
        public decimal BuyM2M { get; }
        public decimal LastPrice { get; }
        public string TradingSymbol { get; }
        public decimal Realised { get; }
        public decimal PNL { get; }
        public decimal Multiplier { get; }
        public int SellQuantity { get; }
        public decimal SellM2M { get; }
        public decimal BuyValue { get; }
        public int BuyQuantity { get; }
        public decimal AveragePrice { get; }
        public decimal Unrealised { get; }
        public decimal Value { get; }
        public decimal BuyPrice { get; }
        public decimal SellPrice { get; }
        public decimal M2M { get; }
        public UInt32 InstrumentToken { get; }
        public decimal ClosePrice { get; }
        public int Quantity { get; }
        public int DayBuyQuantity { get; }
        public decimal DayBuyPrice { get; }
        public decimal DayBuyValue { get; }
        public int DaySellQuantity { get; }
        public decimal DaySellPrice { get; }
        public decimal DaySellValue { get; }
    }
}
