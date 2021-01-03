using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct Historical
    {
        public Historical(ArrayList data)
        {
            TimeStamp = Convert.ToDateTime(data[0]);
            Open = Convert.ToDecimal(data[1]);
            High = Convert.ToDecimal(data[2]);
            Low = Convert.ToDecimal(data[3]);
            Close = Convert.ToDecimal(data[4]);
            Volume = Convert.ToUInt32(data[5]);
            OI = data.Count > 6 ? Convert.ToUInt32(data[6]) : 0;
        }

        public DateTime TimeStamp { get; }
        public decimal Open { get; }
        public decimal High { get; }
        public decimal Low { get; }
        public decimal Close { get; }
        public UInt32 Volume { get; }
        public UInt32 OI { get; }
    }
}
