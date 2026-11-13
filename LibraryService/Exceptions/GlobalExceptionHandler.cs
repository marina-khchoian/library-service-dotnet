using LibraryService.DTOs.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace LibraryService.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext http, Exception exception, CancellationToken ct)
        {
            switch (exception)
            {
                case NotFoundException notFoundException:
                    http.Response.StatusCode = StatusCodes.Status404NotFound;
                    await http.Response.WriteAsJsonAsync(new ErrorResponse
                    {
                        Status = http.Response.StatusCode,
                        Message = notFoundException.Message
                    }, cancellationToken: ct);
                    
                    return true;


                    case BusinessRuleException businessRuleException:
                    http.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await http.Response.WriteAsJsonAsync(new ErrorResponse
                    {
                        Status = http.Response.StatusCode,
                        Message = businessRuleException.Message
                    }, cancellationToken: ct);

                    return true;

                    case ForbiddenException forbiddenException:
                    http.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await http.Response.WriteAsJsonAsync(new ErrorResponse
                    {
                        Status = http.Response.StatusCode,
                        Message = forbiddenException.Message
                    }, cancellationToken: ct);
                    return true;

                default:
                    _logger.LogError(exception, "Unhandled exception");
                    http.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    await http.Response.WriteAsJsonAsync(new ErrorResponse
                    {
                        Status = http.Response.StatusCode,
                        Message = "An unexpected error occurred."
                    }, cancellationToken: ct);
                    return true;
            }
        }
    }
}
