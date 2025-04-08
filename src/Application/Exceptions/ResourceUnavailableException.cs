using System;

namespace EventManagement.Application.Exceptions
{
    /// <summary>
    /// Exception thrown when a resource is unavailable, such as when a session or event is fully booked
    /// </summary>
    public class ResourceUnavailableException : ApplicationException
    {
        public string ResourceType { get; }
        public string ResourceId { get; }

        public ResourceUnavailableException(string resourceType, string resourceId, string message) 
            : base(message)
        {
            ResourceType = resourceType;
            ResourceId = resourceId;
        }

        public ResourceUnavailableException(string resourceType, string resourceId, string message, Exception innerException) 
            : base(message, innerException)
        {
            ResourceType = resourceType;
            ResourceId = resourceId;
        }
    }
}