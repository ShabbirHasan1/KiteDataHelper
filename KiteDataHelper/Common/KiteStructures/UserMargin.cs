using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using KiteDataHelper.Common.KiteExceptions;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct UserMargin
    {
        public UserMargin(Dictionary<string, dynamic> data)
        {
            try
            {
                Enabled = data["enabled"];
                Net = data["net"];
                Available = new AvailableMargin(data["available"]);
                Utilised = new UtilisedMargin(data["utilised"]);
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }

        public bool Enabled { get; set; }
        public decimal Net { get; set; }
        public AvailableMargin Available { get; set; }
        public UtilisedMargin Utilised { get; set; }
    }
}
