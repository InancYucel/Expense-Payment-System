using System.Net;
using System.Text.Json;

namespace Expense_Payment_System.Middleware;

public class WitcherMiddleware
{
    private readonly RequestDelegate next;

    public WitcherMiddleware(RequestDelegate next)
    {
        this.next = next;
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

        await next.Invoke(context);
    }
}