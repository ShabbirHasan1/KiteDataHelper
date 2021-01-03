using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct GTTOrder
    {
        public GTTOrder(Dictionary<string, dynamic> data)
        {
            try
            {
                TransactionType = data["transaction_type"];
                Product = data["product"];
                OrderType = data["order_type"];
                Quantity = Convert.ToInt32(data["quantity"]);
                Price = data["price"];
                Result = data["result"] == null ? null : new Nullable<GTTResult>(new GTTResult(data["result"]));
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }

        public string TransactionType { get; set; }
        public string Product { get; set; }
        public string OrderType { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public GTTResult? Result { get; set; }
    }
}
