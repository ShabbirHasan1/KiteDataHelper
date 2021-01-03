using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct GTTOrderResult
    {
        public GTTOrderResult(Dictionary<string, dynamic> data)
        {
            try
            {
                OrderId = data["order_id"];
                RejectionReason = data["rejection_reason"];
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }

        public string OrderId { get; set; }
        public string RejectionReason { get; set; }
    }
}
