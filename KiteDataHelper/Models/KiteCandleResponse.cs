using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Models
{
    public class KiteCandleResponse
    {
        public KiteCandleResponse()
        {
            this.data = new KiteCandleData();
        }

        public string status { get; set; }
        public KiteCandleData data { get; set; }
    }

    public class KiteCandleData
    {
        private List<ArrayList> _candles;
        public KiteCandleData()
        {
            this._candles = new List<ArrayList>();
        }
        public List<ArrayList> candles { get; set; }
    }
}
