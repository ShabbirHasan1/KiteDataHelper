using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteExceptions
{
    public class KiteGeneralException : KiteException
    {
        public KiteGeneralException(string Message, HttpStatusCode HttpStatus = HttpStatusCode.InternalServerError, Exception innerException = null) : base(Message, HttpStatus, innerException) { }
    }
}
