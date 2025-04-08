using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Models
{
    /// <summary>
    /// Represents a standardized problem details response following RFC 7807
    /// </summary>
    public class ApiError : ProblemDetails
    {
        /// <summary>
        /// A machine-readable identifier for this specific error occurrence
        /// </summary>
        public string ErrorId { get; set; }

        /// <summary>
        /// A URL to documentation specific to this error
        /// </summary>
        public string DocumentationUrl { get; set; }

        /// <summary>
        /// The HTTP method used in the failed request
        /// </summary>
        public string Method { get; set; }
    }
}