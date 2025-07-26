using System.Net;
using System.Text.Json;

namespace sudoku_solver_api.Helpers;

public class ExceptionMiddleware
{
  private readonly RequestDelegate _next;

  public ExceptionMiddleware(RequestDelegate next)
  {
    _next = next;
  }

  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await _next(context);
    }
    catch (Exception ex)
    {
      await HandleExceptionAsync(context, ex);
      context.Response.ContentType = "application/json";
      context.Response.StatusCode = 500;

      var response = new { message = ex.Message };
      await context.Response.WriteAsJsonAsync(response);
    }
  }

  private static Task HandleExceptionAsync(HttpContext context, Exception ex)
  {
    var statusCode = ex switch
    {
      ArgumentNullException => HttpStatusCode.BadRequest,
      ArgumentException => HttpStatusCode.BadRequest,
      InvalidOperationException => HttpStatusCode.Conflict,
      UnauthorizedAccessException => HttpStatusCode.Unauthorized,
      NotImplementedException => HttpStatusCode.NotImplemented,
      _ => HttpStatusCode.InternalServerError
    };

    var response = new
    {
      Message = ex.Message,
      StatusCode = (int)statusCode
    };

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = (int)statusCode;

    return context.Response.WriteAsync(JsonSerializer.Serialize(response));
  }
}