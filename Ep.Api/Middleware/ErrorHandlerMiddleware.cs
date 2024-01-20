using System.Net;
using System.Text.Json;
using Serilog;

namespace Expense_Payment_System.Middleware;

public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next) //Dependency Injection for Request Delegate
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context); //The next middleware is called with the Invoke() command line.
        }
        catch (Exception e) //Every RunTime error in our program will fall here thanks to Middleware
        {
            Log.Error(e, "UnExceptedError"); 
            Log.Fatal(
                $"Path={context.Request.Path} || " +
                $"Method={context.Request.Method} || " + 
                $"Exception={e.Message}"
                // The Path, Method and Message where the error occurred will be transmitted thanks to the above settings.
            );
            
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize("Internal Error"));
        }
    }
}