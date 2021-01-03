using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct GTTResult
    {
        public GTTResult(Dictionary<string, dynamic> data)
        {
            try
            {
                OrderResult = data["order_result"] == null ? null : new Nullable<GTTOrderResult>(new GTTOrderResult(data["order_result"]));
                Timestamp = data["timestamp"];
                TriggeredAtPrice = data["triggered_at"];
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }

        public GTTOrderResult? OrderResult { get; set; }
        public string Timestamp { get; set; }
        public decimal TriggeredAtPrice { get; set; }
    }
}
