using System.ComponentModel;

namespace KiteDataHelper.Common.Enums
{
    public enum KiteOrderType
    {
        [Description("MARKET")]
        Market,
        [Description("LIMIT")]
        Limit,
        [Description("SL")]
        StopLoss,
        [Description("SL-M")]
        StopLossMarket
    }
}
