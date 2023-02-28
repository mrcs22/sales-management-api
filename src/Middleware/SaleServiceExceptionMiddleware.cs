using tech_test_payment_api.Services.Exceptions;
using System.Text.Json;

namespace tech_test_payment_api.Middleware
{
   public class SaleServiceExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public SaleServiceExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SaleServiceException ex)
        {
            var response = context.Response;
            response.ContentType = "application/json";

            context.Response.StatusCode = 400;

            if(ex.GetType() == typeof(SaleNotFoundException))
                context.Response.StatusCode = 404;
            
            var json = JsonSerializer.Serialize(new { message = ex?.Message });
            await context.Response.WriteAsync(json);
        }
        
    }
}
}