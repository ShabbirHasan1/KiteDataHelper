using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct User
    {
        public User(Dictionary<string, dynamic> data)
        {
            try
            {
                APIKey = data["data"]["api_key"];
                Products = (string[])data["data"]["products"].ToArray(typeof(string));
                UserName = data["data"]["user_name"];
                UserShortName = data["data"]["user_shortname"];
                AvatarURL = data["data"]["avatar_url"];
                Broker = data["data"]["broker"];
                AccessToken = data["data"]["access_token"];
                PublicToken = data["data"]["public_token"];
                RefreshToken = data["data"]["refresh_token"];
                UserType = data["data"]["user_type"];
                UserId = data["data"]["user_id"];
                LoginTime = CommonFunctions.StringToDate(data["data"]["login_time"]);
                Exchanges = (string[])data["data"]["exchanges"].ToArray(typeof(string));
                OrderTypes = (string[])data["data"]["order_types"].ToArray(typeof(string));
                Email = data["data"]["email"];
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }

        }

        public string APIKey { get; }
        public string[] Products { get; }
        public string UserName { get; }
        public string UserShortName { get; }
        public string AvatarURL { get; }
        public string Broker { get; }
        public string AccessToken { get; }
        public string PublicToken { get; }
        public string RefreshToken { get; }
        public string UserType { get; }
        public string UserId { get; }
        public DateTime? LoginTime { get; }
        public string[] Exchanges { get; }
        public string[] OrderTypes { get; }
        public string Email { get; }
    }
}
