using System;

namespace EventManagement.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when a user tries to access a resource they are not authorized to access
    /// </summary>
    public class UnauthorizedException : ApplicationException
    {
        public UnauthorizedException(string message) : base(message)
        {
        }

        public UnauthorizedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}