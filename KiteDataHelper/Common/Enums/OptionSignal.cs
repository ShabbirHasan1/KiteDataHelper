using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KiteDataHelper.Common.Enums
{
    public enum OptionSignal
    {
        [Description("Buy Call")]
        EntryCall,
        [Description("Square Off Call")]
        ExitCall,
        [Description("Buy Put")]
        EntryPut,
        [Description("Square Off Put")]
        ExitPut,
        [Description("Trail Stop Loss")]
        TrailStopLoss
    }
}
