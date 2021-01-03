using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KiteDataHelper.Common.KiteExceptions;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct AvailableMargin
    {
        public AvailableMargin(Dictionary<string, dynamic> data)
        {
            try
            {
                AdHocMargin = data["adhoc_margin"];
                Cash = data["cash"];
                Collateral = data["collateral"];
                IntradayPayin = data["intraday_payin"];
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }

        public decimal AdHocMargin { get; set; }
        public decimal Cash { get; set; }
        public decimal Collateral { get; set; }
        public decimal IntradayPayin { get; set; }
    }
}
