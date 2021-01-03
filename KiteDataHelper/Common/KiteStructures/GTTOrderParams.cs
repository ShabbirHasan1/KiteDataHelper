using System;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct GTTOrderParams
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        // Order type (LIMIT, SL, SL-M, MARKET)
        public string OrderType { get; set; }
        // Product code (NRML, MIS, CNC)
        public string Product { get; set; }
        // Transaction type (BUY, SELL)
        public string TransactionType { get; set; }
    }
}
