using System;

namespace DynamicProxy.Exceptions
{
    public class ConnectionException : Exception
    {

        private string uri;
        public ConnectionException(string uri)
        {
            this.uri = uri;
        }

        public override string Message
        {
            get
            {
                return "WebApiProxy: Could not connect to remote server - " + uri;
            }
        }
    }


}
