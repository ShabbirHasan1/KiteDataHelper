using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteExceptions
{
    public class KiteDataException : KiteException
    {
        public KiteDataException(string Message, HttpStatusCode HttpStatus = HttpStatusCode.BadGateway, Exception innerException = null) : base(Message, HttpStatus, innerException) { }
    }
}
