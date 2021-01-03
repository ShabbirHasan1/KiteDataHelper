using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct MFOrder
    {
        public MFOrder(Dictionary<string, dynamic> data)
        {
            try
            {
                StatusMessage = data["status_message"];
                PurchaseType = data["purchase_type"];
                PlacedBy = data["placed_by"];
                Amount = data["amount"];
                Quantity = data["quantity"];
                SettlementId = data["settlement_id"];
                OrderTimestamp = CommonFunctions.StringToDate(data["order_timestamp"]);
                AveragePrice = data["average_price"];
                TransactionType = data["transaction_type"];
                ExchangeOrderId = data["exchange_order_id"];
                ExchangeTimestamp = CommonFunctions.StringToDate(data["exchange_timestamp"]);
                Fund = data["fund"];
                Variety = data["variety"];
                Folio = data["folio"];
                Tradingsymbol = data["tradingsymbol"];
                Tag = data["tag"];
                OrderId = data["order_id"];
                Status = data["status"];
                LastPrice = data["last_price"];
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }

        public string StatusMessage { get; }
        public string PurchaseType { get; }
        public string PlacedBy { get; }
        public decimal Amount { get; }
        public decimal Quantity { get; }
        public string SettlementId { get; }
        public DateTime? OrderTimestamp { get; }
        public decimal AveragePrice { get; }
        public string TransactionType { get; }
        public string ExchangeOrderId { get; }
        public DateTime? ExchangeTimestamp { get; }
        public string Fund { get; }
        public string Variety { get; }
        public string Folio { get; }
        public string Tradingsymbol { get; }
        public string Tag { get; }
        public string OrderId { get; }
        public string Status { get; }
        public decimal LastPrice { get; }
    }
}
