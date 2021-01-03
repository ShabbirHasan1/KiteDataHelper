using KiteDataHelper.Common.KiteExceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteStructures
{
    public struct TokenSet
    {
        public TokenSet(Dictionary<string, dynamic> data)
        {
            try
            {
                UserId = data["data"]["user_id"];
                AccessToken = data["data"]["access_token"];
                RefreshToken = data["data"]["refresh_token"];
            }
            catch (Exception e)
            {
                throw new KiteDataException("Unable to parse data. " + CommonFunctions.JsonSerialize(data), HttpStatusCode.OK, e);
            }
        }
        public string UserId { get; }
        public string AccessToken { get; }
        public string RefreshToken { get; }
    }
}
