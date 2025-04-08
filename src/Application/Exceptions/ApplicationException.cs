using System;

namespace EventManagement.Application.Exceptions
{
    /// <summary>
    /// Base exception class for the application domain
    /// </summary>
    public abstract class ApplicationException : Exception
    {
        protected ApplicationException(string message) : base(message)
        {
        }

        protected ApplicationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}