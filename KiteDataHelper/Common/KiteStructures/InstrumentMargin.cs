using System;
using System.Collections.Generic;
using System.Text;
using KiteDataHelper.Common.KiteExceptions;
using System.Net;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct InstrumentMargin
    {
        public InstrumentMargin(Dictionary<string, dynamic> data)
        {
            try
            {
                Margin = data["margin"];
                COLower = data["co_lower"];
                MISMultiplier = data["mis_multiplier"];
                Tradingsymbol = data["tradingsymbol"];
                COUpper = data["co_upper"];
                NRMLMargin = data["nrml_margin"];
                MISMargin = data["mis_margin"];
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }

        public string Tradingsymbol { get; set; }
        public decimal Margin { get; set; }
        public decimal COLower { get; set; }
        public decimal COUpper { get; set; }
        public decimal MISMultiplier { get; set; }
        public decimal MISMargin { get; set; }
        public decimal NRMLMargin { get; set; }
    }
}
