using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace EventManagement.API.Models
{
    /// <summary>
    /// Provides error code generation and validation for consistent error handling across the application
    /// </summary>
    public static class ErrorCodes
    {
        private static readonly Regex ErrorCodePattern = new Regex(
            @"^EM\d{4}$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant
        );

        // Common error prefixes by functional area
        private const string GeneralPrefix = "EM0";      // General errors 
        private const string EventPrefix = "EM1";        // Event-related errors
        private const string ParticipantPrefix = "EM2";  // Participant-related errors  
        private const string SessionPrefix = "EM3";      // Session-related errors
        private const string SpeakerPrefix = "EM4";      // Speaker-related errors
        private const string LocationPrefix = "EM5";     // Location-related errors
        private const string AuthPrefix = "EM6";         // Authentication/Authorization errors
        private const string ValidationPrefix = "EM7";   // Validation errors
        private const string DataPrefix = "EM8";         // Database/data errors
        private const string IntegrationPrefix = "EM9";  // Third-party integration errors

        /// <summary>
        /// Dictionary of error codes and their descriptions
        /// </summary>
        public static readonly Dictionary<string, string> ErrorMessages = new Dictionary<string, string>
        {
            // General errors (EM00xx)
            { "EM0001", "An unexpected error occurred" },
            { "EM0002", "Invalid request format" },
            { "EM0003", "Resource not found" },
            { "EM0004", "Operation timed out" },
            { "EM0005", "Concurrent modification detected" },
            
            // Event errors (EM10xx)
            { "EM1001", "Event not found" },
            { "EM1002", "Invalid event dates" },
            { "EM1003", "Event is already at capacity" },
            { "EM1004", "Event has already started or ended" },
            { "EM1005", "Cannot delete event with registered participants" },
            
            // Participant errors (EM20xx)
            { "EM2001", "Participant not found" },
            { "EM2002", "Participant already registered for this event" },
            { "EM2003", "Participant email already exists" },
            { "EM2004", "Cannot unregister from an event that has already started" },
            
            // Session errors (EM30xx)
            { "EM3001", "Session not found" },
            { "EM3002", "Invalid session times" },
            { "EM3003", "Room capacity exceeded" },
            { "EM3004", "Session scheduling conflict" },
            { "EM3005", "Cannot rate a session that has not occurred yet" },
            
            // Speaker errors (EM40xx)
            { "EM4001", "Speaker not found" },
            { "EM4002", "Speaker scheduling conflict" },
            
            // Location/Room errors (EM50xx)
            { "EM5001", "Location not found" },
            { "EM5002", "Room not found" },
            { "EM5003", "Room is already booked for this time" },
            
            // Auth errors (EM60xx)
            { "EM6001", "Unauthorized access" },
            { "EM6002", "Authentication failed" },
            { "EM6003", "Session expired" },
            
            // Validation errors (EM70xx)
            { "EM7001", "Validation failed" },
            { "EM7002", "Required field missing" },
            { "EM7003", "Value out of allowed range" },
            
            // Data errors (EM80xx)
            { "EM8001", "Database operation failed" },
            { "EM8002", "Data integrity violation" },
            
            // Integration errors (EM90xx)
            { "EM9001", "Third-party service unavailable" },
            { "EM9002", "Payment processing failed" }
        };

        /// <summary>
        /// Validates if the provided string is a valid error code format
        /// </summary>
        /// <param name="code">The error code to validate</param>
        /// <returns>True if the code matches the expected pattern</returns>
        public static bool IsValidErrorCode(string code)
        {
            return !string.IsNullOrEmpty(code) && ErrorCodePattern.IsMatch(code);
        }

        /// <summary>
        /// Gets the error message for a specific error code
        /// </summary>
        /// <param name="code">The error code</param>
        /// <returns>The corresponding error message or a default message if not found</returns>
        public static string GetErrorMessage(string code)
        {
            if (ErrorMessages.TryGetValue(code, out var message))
            {
                return message;
            }
            
            return "An error occurred. Please contact support if the issue persists.";
        }

        /// <summary>
        /// Creates an error code in the correct format
        /// </summary>
        /// <param name="category">The error category (0-9)</param>
        /// <param name="code">The error code (00-99)</param>
        /// <returns>A formatted error code, e.g., EM101</returns>
        public static string CreateErrorCode(int category, int code)
        {
            if (category < 0 || category > 9)
            {
                throw new System.ArgumentOutOfRangeException(nameof(category), "Category must be between 0 and 9");
            }
            
            if (code < 0 || code > 99)
            {
                throw new System.ArgumentOutOfRangeException(nameof(code), "Code must be between 0 and 99");
            }
            
            return string.Format(CultureInfo.InvariantCulture, "EM{0}{1:D2}", category, code);
        }
    }
}