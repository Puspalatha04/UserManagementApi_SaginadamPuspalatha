
namespace UserManagementAPI.Middleware;

public class LoggingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        await next(context);

        Console.WriteLine(
            $"{DateTime.Now:dd-MM-yyyy HH:mm:ss} | {context.Request.Method} {context.Request.Path} → {context.Response.StatusCode}"
        );
    }
}
