using System;

namespace EventManagement.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested operation could not be completed due to a timeout
    /// </summary>
    public class TimeoutException : ApplicationException
    {
        public TimeoutException(string message) : base(message)
        {
        }

        public TimeoutException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}