using System;
using System.Collections.Generic;
using System.Text;

namespace KiteDataHelper.Common.Exceptions
{
    public class IntelliTradeException : Exception
    {
        public IntelliTradeException(string Message, Exception innerException = null) : base(Message, innerException) 
        {  
        }
    }
}
