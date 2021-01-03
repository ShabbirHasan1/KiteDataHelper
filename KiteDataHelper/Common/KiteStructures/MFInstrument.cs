using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct MFInstrument
    {
        public MFInstrument(Dictionary<string, dynamic> data)
        {
            try
            {
                TradingSymbol = data["tradingsymbol"];
                AMC = data["amc"];
                Name = data["name"];

                PurchaseAllowed = data["purchase_allowed"] == "1";
                RedemtpionAllowed = data["redemption_allowed"] == "1";

                MinimumPurchaseAmount = CommonFunctions.StringToDecimal(data["minimum_purchase_amount"]);
                PurchaseAmountMultiplier = CommonFunctions.StringToDecimal(data["purchase_amount_multiplier"]);
                MinimumAdditionalPurchaseAmount = CommonFunctions.StringToDecimal(data["minimum_additional_purchase_amount"]);
                MinimumRedemptionQuantity = CommonFunctions.StringToDecimal(data["minimum_redemption_quantity"]);
                RedemptionQuantityMultiplier = CommonFunctions.StringToDecimal(data["redemption_quantity_multiplier"]);
                LastPrice = CommonFunctions.StringToDecimal(data["last_price"]);

                DividendType = data["dividend_type"];
                SchemeType = data["scheme_type"];
                Plan = data["plan"];
                SettlementType = data["settlement_type"];
                LastPriceDate = CommonFunctions.StringToDate(data["last_price_date"]);
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }

        }

        public string TradingSymbol { get; }
        public string AMC { get; }
        public string Name { get; }

        public bool PurchaseAllowed { get; }
        public bool RedemtpionAllowed { get; }

        public decimal MinimumPurchaseAmount { get; }
        public decimal PurchaseAmountMultiplier { get; }
        public decimal MinimumAdditionalPurchaseAmount { get; }
        public decimal MinimumRedemptionQuantity { get; }
        public decimal RedemptionQuantityMultiplier { get; }
        public decimal LastPrice { get; }

        public string DividendType { get; }
        public string SchemeType { get; }
        public string Plan { get; }
        public string SettlementType { get; }
        public DateTime? LastPriceDate { get; }
    }
}
