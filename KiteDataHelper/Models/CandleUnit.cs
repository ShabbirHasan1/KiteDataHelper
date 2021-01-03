using System;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Models
{
    public class CandleUnit
    {
        public double time { get; set; }
        public decimal open { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal close { get; set; }
        public long volume { get; set; }
    }
}
