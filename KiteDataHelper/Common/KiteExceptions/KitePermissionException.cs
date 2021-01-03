using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace KiteDataHelper.Common.KiteExceptions
{
    public class KitePermissionException : KiteException
    {
        public KitePermissionException(string Message, HttpStatusCode HttpStatus = HttpStatusCode.Forbidden, Exception innerException = null) : base(Message, HttpStatus, innerException) { }
    }
}
