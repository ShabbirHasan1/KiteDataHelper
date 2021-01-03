using System.ComponentModel;

namespace KiteDataHelper.Common.Enums
{
    public enum Interval
    {
        [Description("minute")]
        Minute,
        [Description("day")]
        Day,
        [Description("3minute")]
        ThreeMinute,
        [Description("5minute")]
        FiveMinute,
        [Description("10minute")]
        TenMinute,
        [Description("15minute")]
        FifteenMinute,
        [Description("30minute")]
        ThirtyMinute,
        [Description("60minute")]
        SixtyMinute
    }
}
