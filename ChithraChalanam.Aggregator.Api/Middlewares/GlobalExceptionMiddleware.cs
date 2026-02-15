using System.Net;

namespace ChithraChalanam.Aggregator.Api.Middlewares;
public class GlobalExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                Message = "Something went wrong"
            });
        }
    }
}

