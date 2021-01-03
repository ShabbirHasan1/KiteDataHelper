using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteExceptions
{
    public class KiteNetworkException : KiteException
    {
        public KiteNetworkException(string Message, HttpStatusCode HttpStatus = HttpStatusCode.ServiceUnavailable, Exception innerException = null) : base(Message, HttpStatus, innerException) { }
    }
}
