using System.Net;
using System.Text.Json;

namespace Expense_Payment_System.Middleware;

public class WitcherMiddleware
{
    private readonly RequestDelegate _next;

    public WitcherMiddleware(RequestDelegate next) //Dependency Injection for Request Delegate
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/hello"))
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.OK; 
            await context.Response.WriteAsync(JsonSerializer.Serialize("Hello from server"));
            return;
        }
        await _next.Invoke(context);// The next middleware is called with the Invoke() command line.
    }
}