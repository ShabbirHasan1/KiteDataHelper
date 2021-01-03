using System;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Models
{
    [Serializable]
    public class KiteAccessTokemResponseData
    {
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string user_shortname { get; set; }
        public string email { get; set; }
        public string user_type { get; set; }
        public string broker { get; set; }
        public List<string> exchanges { get; set; }
        public List<string> products { get; set; }
        public List<string> order_types { get; set; }
        public string api_key { get; set; }
        public string access_token { get; set; }
        public string public_token { get; set; }
        public string login_time { get; set; }
    }
}
