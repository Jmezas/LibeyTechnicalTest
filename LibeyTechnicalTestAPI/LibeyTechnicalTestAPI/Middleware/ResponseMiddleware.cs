using System.Net;
using System.Text.Json;
using LibeyTechnicalTestDomain.LibeyUserAggregate.Application.DTO;

namespace LibeyTechnicalTestAPI.Middleware;

public class ResponseMiddleware
{
    private readonly RequestDelegate _next;

    public ResponseMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using var newBodyStream = new MemoryStream();
        context.Response.Body = newBodyStream;

        await _next(context);

        newBodyStream.Seek(0, SeekOrigin.Begin);
        var responseBody = await new StreamReader(newBodyStream).ReadToEndAsync();

        var apiResponse = new
        {
            success = context.Response.StatusCode is >= 200 and < 300,
            statusCode = context.Response.StatusCode,
            data = !string.IsNullOrWhiteSpace(responseBody) ? JsonSerializer.Deserialize<object>(responseBody) : null
        };

        context.Response.ContentType = "application/json";
        context.Response.Body = originalBodyStream;

        await context.Response.WriteAsync(JsonSerializer.Serialize(apiResponse));
    }
}
