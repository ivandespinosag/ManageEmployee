using ManageEmployees.Core.DTOs;
using System.Net;
using System.Text.Json;

namespace ManageEmployees.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"Oooops! Algo salió mal: {ex.Message}");
                await HandleGlobalExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleGlobalExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(JsonSerializer.Serialize(new ResponseDto()
            {
                StatusCode = StatusCodes.Status500InternalServerError, //Custom error
                IsSuccess = false,
                Message = $"Algo salió mal. Error!: {exception.Message}",
                Result = exception.StackTrace ?? string.Empty
            })); ;
        }
    }
}
