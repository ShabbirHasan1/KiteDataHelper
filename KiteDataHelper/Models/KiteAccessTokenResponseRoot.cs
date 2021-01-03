using System;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Models
{
    [Serializable]
    public class KiteAccessTokenResponseRoot
    {
        public string status { get; set; }
        public KiteAccessTokemResponseData data { get; set; }
    }
}
