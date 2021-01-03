using System;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Models
{
    public class KiteAccessTokenRequest
    {
        public string api_key { get; set; }
        public string request_token { get; set; }

        public string checksum { get; set; }
    }
}
