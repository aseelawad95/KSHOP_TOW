using KSHOP_TWO.DAL.DTO.Response;
using System.Net;
using System.Text.Json;

namespace KSHOP_TOW.PL.Middleware
{
    public class GlobalExceptionHandling
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandling(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentNullException ex)
            {
                await HandleExceptionAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    "Required value is missing.",
                    ex.Message
                );
            }
            catch (ArgumentException ex)
            {
                await HandleExceptionAsync(
                    context,
                    StatusCodes.Status400BadRequest,
                    "Invalid argument provided.",
                    ex.Message
                );
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(
                    context,
                    StatusCodes.Status404NotFound,
                    "Resource not found.",
                    ex.Message
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(
                    context,
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized access.",
                    ex.Message
                );
            }
            catch (InvalidOperationException ex)
            {
                await HandleExceptionAsync(
                    context,
                    StatusCodes.Status409Conflict,
                    "Invalid operation.",
                    ex.Message
                );
            }
            catch (TimeoutException ex)
            {
                await HandleExceptionAsync(
                    context,
                    StatusCodes.Status408RequestTimeout,
                    "Request timeout.",
                    ex.Message
                );
            }
            catch (NotImplementedException ex)
            {
                await HandleExceptionAsync(
                    context,
                    StatusCodes.Status501NotImplemented,
                    "Feature not implemented.",
                    ex.Message
                );
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "Server Error.",
                    ex.InnerException?.Message ?? ex.Message
                );
            }
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            int statusCode,
            string message,
            string? innerError)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var errorDetails = new ErrorDetails
            {
                StatusCode = statusCode,
                Message = message,
                InnerError = innerError
            };

            await context.Response.WriteAsJsonAsync(errorDetails);
        }
    }
}