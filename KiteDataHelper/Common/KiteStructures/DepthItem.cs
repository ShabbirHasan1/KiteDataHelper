using System;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct DepthItem
    {
        public DepthItem(Dictionary<string, dynamic> data)
        {
            Quantity = Convert.ToUInt32(data["quantity"]);
            Price = data["price"];
            Orders = Convert.ToUInt32(data["orders"]);
        }

        public UInt32 Quantity { get; set; }
        public decimal Price { get; set; }
        public UInt32 Orders { get; set; }
    }
}
