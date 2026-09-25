using ProjetoFinal.API.Contracts;
using ProjetoFinal.API.Exceptions;

namespace ProjetoFinal.API.Middlewares;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                "Unhandled exception. Type: {ExceptionType}; Method: {Method}; Path: {Path}",
                exception.GetType().Name,
                context.Request.Method,
                context.Request.Path);

            await HandleExceptionAsync(context, exception);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = exception switch
        {
            RequestValidationException validation => new ApiErrorResponse(400, "VALIDATION_ERROR", validation.Message),
            ResourceNotFoundException notFound => new ApiErrorResponse(404, "NOT_FOUND", notFound.Message),
            BusinessRuleViolationException businessRule => new ApiErrorResponse(409, "BUSINESS_RULE_VIOLATION", businessRule.Message),
            _ => new ApiErrorResponse(500, "INTERNAL_ERROR", "Ocorreu um erro interno no servidor.")
        };

        context.Response.StatusCode = response.Status;
        return context.Response.WriteAsJsonAsync(response);
    }
}
