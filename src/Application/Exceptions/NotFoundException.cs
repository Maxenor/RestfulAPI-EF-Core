using System;

namespace EventManagement.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when a requested entity is not found
    /// </summary>
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object key)
            : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }
    }
}