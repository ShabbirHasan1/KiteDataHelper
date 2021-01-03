using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteExceptions
{
    public class KiteException : Exception
    {
        HttpStatusCode Status;
        public KiteException(string Message, HttpStatusCode HttpStatus, Exception innerException = null) : base(Message, innerException) { Status = HttpStatus; }
    }
}
