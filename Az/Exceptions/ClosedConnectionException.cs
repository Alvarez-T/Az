using System;

namespace Az.Exceptions
{
    public class ClosedConnectionException : Exception
    {
        public ClosedConnectionException() : base()
        {

        }
        public ClosedConnectionException(string message) : base(message)
        {

        }
    }
}
