using Microsoft.Data.Analysis;
using System;

namespace KiteDataHelper.Models
{
    public class TickCache
    {
        private DataFrame _tickFrame;
        public TickCache()
        {
            PrimitiveDataFrameColumn<DateTime> dateTimes = new PrimitiveDataFrameColumn<DateTime>("timestamp");
            UInt32DataFrameColumn instrumentToken = new UInt32DataFrameColumn("instrumenttoken");
            BooleanDataFrameColumn tradable = new BooleanDataFrameColumn("tradable");
            DecimalDataFrameColumn open = new DecimalDataFrameColumn("open");
            DecimalDataFrameColumn high = new DecimalDataFrameColumn("high");
            DecimalDataFrameColumn low = new DecimalDataFrameColumn("low");
            DecimalDataFrameColumn close = new DecimalDataFrameColumn("close");
            DecimalDataFrameColumn lastPrice = new DecimalDataFrameColumn("lastprice");
            UInt64DataFrameColumn volume = new UInt64DataFrameColumn("volume");
            _tickFrame = new DataFrame(dateTimes, instrumentToken, tradable, open, high, low, close, lastPrice, volume);
        }

        public DataFrame Data
        {
            get
            {
                return _tickFrame;
            }

            private set { }
        }
    }
}
