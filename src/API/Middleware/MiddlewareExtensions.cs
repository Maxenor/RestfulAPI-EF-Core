using Microsoft.AspNetCore.Builder;

namespace EventManagement.API.Middleware
{
    /// <summary>
    /// Extension methods for adding custom middleware to the application pipeline
    /// </summary>
    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Adds the global error handling middleware to the application pipeline
        /// </summary>
        /// <param name="builder">The application builder</param>
        /// <returns>The application builder with error handling middleware configured</returns>
        public static IApplicationBuilder UseGlobalErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}