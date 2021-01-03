using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct TrigerRange
    {
        public TrigerRange(Dictionary<string, dynamic> data)
        {
            try
            {
                InstrumentToken = Convert.ToUInt32(data["instrument_token"]);
                Lower = data["lower"];
                Upper = data["upper"];
                Percentage = data["percentage"];
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }

        }
        public UInt32 InstrumentToken { get; }
        public decimal Lower { get; }
        public decimal Upper { get; }
        public decimal Percentage { get; }
    }
}
