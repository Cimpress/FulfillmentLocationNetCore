using System;

namespace Cimpress.FulfillmentLocationNetCore.Errors
{
    public class AccessTokenException : Exception
    {
        public AccessTokenException() { }
        public AccessTokenException(string message) : base(message) { }
        public AccessTokenException(string message, Exception inner) : base(message, inner) { }
        protected AccessTokenException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
