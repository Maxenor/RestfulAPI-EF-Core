using System;

namespace EventManagement.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when a conflict occurs during an operation
    /// </summary>
    public class ConflictException : ApplicationException
    {
        public ConflictException(string message) 
            : base(message)
        {
        }
    }
}