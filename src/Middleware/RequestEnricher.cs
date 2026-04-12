using Serilog;

namespace SymptomLogger.src.Middleware
{
    public class RequestEnricher
    {
        // This method can be called in middleware to log request details
        public static void LogRequestDetails(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {

            diagnosticContext.Set(
            "ClientIP",
            httpContext.Connection.RemoteIpAddress?.ToString());
            

            /*var request = context.Request;
            var method = request.Method;
            var path = request.Path;
            var queryString = request.QueryString.HasValue ? request.QueryString.Value : string.Empty;
            var userAgent = request.Headers["User-Agent"].ToString();
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";*/

        }
    }
}
