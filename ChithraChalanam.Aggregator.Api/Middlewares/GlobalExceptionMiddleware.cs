using System.Net;

namespace ChithraChalanam.Aggregator.Api.Middlewares;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate next;

    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                Message = ex.Message,
                StackTrace = ex.StackTrace
            });
        }
    }
}
