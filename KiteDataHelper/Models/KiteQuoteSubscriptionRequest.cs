using System;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Models
{
    [Serializable]
    public class KiteQuoteSubscriptionRequest
    {
        public List<int> _v;

        public KiteQuoteSubscriptionRequest()
        {
            this.v = new List<int>();
        }
        public string a { get; set; }
        public List<int> v 
        {
            get
            {
                return this._v;
            }

            set
            {
                this._v = value;
            }
        }
    }
}
