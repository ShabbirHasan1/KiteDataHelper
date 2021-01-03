using System;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Models
{
    [Serializable]
    public class KiteTickRequest
    {
        public List<KiteQuoteSubscriptionRequest> v;
        public KiteTickRequest()
        {
            this.v = new List<KiteQuoteSubscriptionRequest>();
        }
        public string a { get; set; }
        public List<KiteQuoteSubscriptionRequest> V 
        {
            get
            {
                return this.v;
            }

            set
            {
                this.v = value;
            }
        }
    }
}
