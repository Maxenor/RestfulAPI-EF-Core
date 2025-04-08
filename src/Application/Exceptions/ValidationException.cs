using System;
using System.Collections.Generic;

namespace EventManagement.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when a validation error occurs
    /// </summary>
    public class ValidationException : ApplicationException
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException() 
            : base("One or more validation failures have occurred.")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IDictionary<string, string[]> errors) 
            : this()
        {
            Errors = errors;
        }

        public ValidationException(string propertyName, string errorMessage) 
            : this()
        {
            Errors.Add(propertyName, new string[] { errorMessage });
        }
    }
}