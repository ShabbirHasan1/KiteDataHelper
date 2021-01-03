using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct Trade
    {
        public Trade(Dictionary<string, dynamic> data)
        {
            try
            {
                TradeId = data["trade_id"];
                OrderId = data["order_id"];
                ExchangeOrderId = data["exchange_order_id"];
                Tradingsymbol = data["tradingsymbol"];
                Exchange = data["exchange"];
                InstrumentToken = Convert.ToUInt32(data["instrument_token"]);
                TransactionType = data["transaction_type"];
                Product = data["product"];
                AveragePrice = data["average_price"];
                Quantity = Convert.ToInt32(data["quantity"]);
                FillTimestamp = CommonFunctions.StringToDate(data["fill_timestamp"]);
                ExchangeTimestamp = CommonFunctions.StringToDate(data["exchange_timestamp"]);
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }

        }

        public string TradeId { get; }
        public string OrderId { get; }
        public string ExchangeOrderId { get; }
        public string Tradingsymbol { get; }
        public string Exchange { get; }
        public UInt32 InstrumentToken { get; }
        public string TransactionType { get; }
        public string Product { get; }
        public decimal AveragePrice { get; }
        public int Quantity { get; }
        public DateTime? FillTimestamp { get; }
        public DateTime? ExchangeTimestamp { get; }
    }

}
