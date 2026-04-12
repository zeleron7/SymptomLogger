using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SymptomLogger.src.Utils;

namespace SymptomLogger.src.Middleware
{
    internal sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {

            _logger.LogError(
               exception, 
               "Error occurred: {Message} at {Time}",
               exception.Message,
               DateTime.UtcNow);
            
            var problemDetails = new ProblemDetails
            {
                Status = exception switch
                {
                    NotFoundException => StatusCodes.Status404NotFound,
                    ArgumentException => StatusCodes.Status400BadRequest,
                    _ => StatusCodes.Status500InternalServerError
                },
                Title = exception switch
                {
                    NotFoundException => "Not found",
                    ArgumentException => "Bad request",
                    _ => "Server error"
                },
                Detail = exception.Message
            };

            httpContext.Response.StatusCode = problemDetails.Status!.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; 
        }
    }
}
