using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KiteDataHelper.Common.KiteExceptions;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct Order
    {
        public Order(Dictionary<string, dynamic> data)
        {
            try
            {
                AveragePrice = data["average_price"];
                CancelledQuantity = Convert.ToInt32(data["cancelled_quantity"]);
                DisclosedQuantity = Convert.ToInt32(data["disclosed_quantity"]);
                Exchange = data["exchange"];
                ExchangeOrderId = data["exchange_order_id"];
                ExchangeTimestamp = CommonFunctions.StringToDate(data["exchange_timestamp"]);
                FilledQuantity = Convert.ToInt32(data["filled_quantity"]);
                InstrumentToken = Convert.ToUInt32(data["instrument_token"]);
                OrderId = data["order_id"];
                OrderTimestamp = CommonFunctions.StringToDate(data["order_timestamp"]);
                OrderType = data["order_type"];
                ParentOrderId = data["parent_order_id"];
                PendingQuantity = Convert.ToInt32(data["pending_quantity"]);
                PlacedBy = data["placed_by"];
                Price = data["price"];
                Product = data["product"];
                Quantity = Convert.ToInt32(data["quantity"]);
                Status = data["status"];
                StatusMessage = data["status_message"];
                Tag = data["tag"];
                Tradingsymbol = data["tradingsymbol"];
                TransactionType = data["transaction_type"];
                TriggerPrice = data["trigger_price"];
                Validity = data["validity"];
                Variety = data["variety"];
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }

        }

        public decimal AveragePrice { get; set; }
        public int CancelledQuantity { get; set; }
        public int DisclosedQuantity { get; set; }
        public string Exchange { get; set; }
        public string ExchangeOrderId { get; set; }
        public DateTime? ExchangeTimestamp { get; set; }
        public int FilledQuantity { get; set; }
        public UInt32 InstrumentToken { get; set; }
        public string OrderId { get; set; }
        public DateTime? OrderTimestamp { get; set; }
        public string OrderType { get; set; }
        public string ParentOrderId { get; set; }
        public int PendingQuantity { get; set; }
        public string PlacedBy { get; set; }
        public decimal Price { get; set; }
        public string Product { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string StatusMessage { get; set; }
        public string Tag { get; set; }
        public string Tradingsymbol { get; set; }
        public string TransactionType { get; set; }
        public decimal TriggerPrice { get; set; }
        public string Validity { get; set; }
        public string Variety { get; set; }
    }
}
