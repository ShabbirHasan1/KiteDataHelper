using System.ComponentModel;

namespace KiteDataHelper.Common.Enums
{
    public enum KiteOrderValidity
    {
        [Description("DAY")]
        Day,
        [Description("IOC")]
        ImmediateOrCancel
    }
}
