using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using TestProj.Core.Common;
using TestProj.Core.Exceptions;

namespace TestProj.API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        string errorMessage = "Xeta bash verdi";

        if (exception is ProductNotFoundException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            errorMessage = "Product tapılmadı";
        }
        else if (exception is ValidationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            errorMessage = "Validasiya xətası";
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }

        var response = new ApiResponse
        {
            ErrorMessage = errorMessage,
            IsSuccess = false
        };

        _logger.LogInformation(errorMessage);

        var jsonResponse = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(jsonResponse);
    }
}
