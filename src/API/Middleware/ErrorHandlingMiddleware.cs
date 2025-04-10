using EventManagement.API.Models;
using EventManagement.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventManagement.API.Middleware
{
    /// <summary>
    /// Middleware for handling exceptions globally and converting them into consistent API responses
    /// following RFC 7807 Problem Details standard with standardized error codes
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IHostEnvironment _environment;

        public ErrorHandlingMiddleware(
            RequestDelegate next, 
            ILogger<ErrorHandlingMiddleware> logger,
            IHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred during request processing: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            string errorCode = "EM0001"; // Default general error
            
            var problem = new ApiError
            {
                Status = (int)statusCode,
                Title = ErrorCodes.GetErrorMessage(errorCode),
                Detail = "An unexpected error occurred while processing your request.",
                DocumentationUrl = "https://api.eventmanagement.com/docs/errors", // Add this line to set the required property
                Instance = context.Request.Path,
                ErrorId = Guid.NewGuid().ToString("N"),
                Method = context.Request.Method
            };

            // Add request ID if available for correlation
            if (context.TraceIdentifier != null)
            {
                problem.Extensions["traceId"] = context.TraceIdentifier;
            }

            // Add error code for better client error handling
            problem.Extensions["errorCode"] = errorCode;

            // Include stack trace in development environment
            if (_environment.IsDevelopment())
            {
                problem.Extensions["stackTrace"] = exception.StackTrace;
            }

            switch (exception)
            {
                case NotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    // Simplified: Use a generic NotFound code. The message provides specifics.
                    errorCode = "EM0003"; // Generic Resource Not Found
                    problem.Title = "Resource not found";
                    problem.Detail = exception.Message;
                    break;

                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = "EM7001";
                    problem.Title = "Validation failed";
                    problem.Detail = "One or more validation errors occurred.";
                    problem.Extensions["errors"] = validationException.Errors;
                    break;

                case ConflictException conflictException:
                    statusCode = HttpStatusCode.Conflict;
                    
                    // Determine appropriate error code based on the message content
                    if (conflictException.Message.Contains("already registered"))
                        errorCode = "EM2002";
                    else if (conflictException.Message.Contains("already started"))
                        errorCode = "EM2004";
                    else if (conflictException.Message.Contains("capacity"))
                        errorCode = "EM1003";
                    else
                        errorCode = "EM0005";
                        
                    problem.Title = "Conflict error";
                    problem.Detail = exception.Message;
                    break;

                case BadRequestException badRequestException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = "EM0002";
                    problem.Title = "Bad request";
                    problem.Detail = exception.Message;
                    break;

                case UnauthorizedException unauthorizedException:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorCode = "EM6001";
                    problem.Title = "Unauthorized";
                    problem.Detail = exception.Message;
                    break;

                case ResourceUnavailableException resourceUnavailableException:
                    statusCode = HttpStatusCode.Conflict;
                    
                    // Determine error code based on resource type
                    errorCode = resourceUnavailableException.ResourceType switch
                    {
                        "Event" => "EM1003",
                        "Room" => "EM5003",
                        "Session" => "EM3003",
                        _ => "EM0003"
                    };
                    
                    problem.Title = "Resource unavailable";
                    problem.Detail = exception.Message;
                    problem.Extensions["resourceType"] = resourceUnavailableException.ResourceType;
                    problem.Extensions["resourceId"] = resourceUnavailableException.ResourceId;
                    break;

                case EventManagement.Application.Exceptions.TimeoutException timeoutException: // Fully qualify
                    statusCode = HttpStatusCode.RequestTimeout;
                    errorCode = "EM0004";
                    problem.Title = "Request timeout";
                    problem.Detail = exception.Message;
                    break;

                case EventManagement.Application.Exceptions.ApplicationException applicationException: // Fully qualify
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = "EM0002";
                    problem.Title = "Application error";
                    problem.Detail = exception.Message;
                    break;

                // For truly unexpected exceptions in production, don't expose details
                default:
                    if (!_environment.IsDevelopment())
                    {
                        problem.Detail = "An unexpected error occurred. Please try again later.";
                    }
                    else
                    {
                        problem.Detail = exception.Message;
                    }
                    break;
            }

            problem.Status = (int)statusCode;
            problem.Title = ErrorCodes.GetErrorMessage(errorCode);
            problem.Extensions["errorCode"] = errorCode;
            
            // Set documentation URL if available
            problem.Type = $"https://api.eventmanagement.example.com/errors/{errorCode}";
            
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem, options));
        }
    }
}