using System;

namespace KiteDataHelper.Common.Delegates
{
    // Delegates for events
    public delegate void OnConnectHandler();
    public delegate void OnCloseHandler();
    public delegate void OnErrorHandler(string Message, Exception ex);
    public delegate void OnDataHandler(byte[] Data, int Count, string MessageType);
}
