using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteExceptions
{
    public class KiteInputException : KiteException
    {
        public KiteInputException(string Message, HttpStatusCode HttpStatus = HttpStatusCode.BadRequest, Exception innerException = null) : base(Message, HttpStatus, innerException) { }
    }
}
