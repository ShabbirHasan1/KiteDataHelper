using System;
using System.Collections.Generic;
using System.Text;
using KiteDataHelper.Common.KiteExceptions;
using System.Net;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct UserMarginsResponse
    {
        public UserMarginsResponse(Dictionary<string, dynamic> data)
        {
            try
            {
                Equity = new UserMargin(data["equity"]);
                Commodity = new UserMargin(data["commodity"]);
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }
        public UserMargin Equity { get; set; }
        public UserMargin Commodity { get; set; }
    }
}
