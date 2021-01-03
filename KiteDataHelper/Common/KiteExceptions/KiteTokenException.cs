using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteExceptions
{
    public class KiteTokenException : KiteException
    {
        public KiteTokenException(string Message, HttpStatusCode HttpStatus = HttpStatusCode.Forbidden, Exception innerException = null) : base(Message, HttpStatus, innerException) { }
    }
}
