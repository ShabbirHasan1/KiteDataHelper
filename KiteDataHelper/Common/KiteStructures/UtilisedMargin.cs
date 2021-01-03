using System;
using System.Collections.Generic;
using System.Net;
using KiteDataHelper.Common.KiteExceptions;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct UtilisedMargin
    {
        public UtilisedMargin(Dictionary<string, dynamic> data)
        {
            try
            {
                Debits = data["debits"];
                Exposure = data["exposure"];
                M2MRealised = data["m2m_realised"];
                M2MUnrealised = data["m2m_unrealised"];
                OptionPremium = data["option_premium"];
                Payout = data["payout"];
                Span = data["span"];
                HoldingSales = data["holding_sales"];
                Turnover = data["turnover"];
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }

        public decimal Debits { get; set; }
        public decimal Exposure { get; set; }
        public decimal M2MRealised { get; set; }
        public decimal M2MUnrealised { get; set; }
        public decimal OptionPremium { get; set; }
        public decimal Payout { get; set; }
        public decimal Span { get; set; }
        public decimal HoldingSales { get; set; }
        public decimal Turnover { get; set; }

    }
}
