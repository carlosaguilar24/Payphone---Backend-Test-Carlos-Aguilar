using System.Net;
using System.Text.Json;
using TransferService.Domain.Exceptions;

namespace TransferService.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (WalletNotFoundException ex)
            {
                await WriteResponseAsync(context, HttpStatusCode.NotFound, ex.Message);
            }
            catch (InsufficientBalanceException ex)
            {
                await WriteResponseAsync(context, HttpStatusCode.UnprocessableEntity, ex.Message);
            }
            catch (InvalidTransferException ex)
            {
                await WriteResponseAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado procesando {Method} {Path}", context.Request.Method, context.Request.Path);
                await WriteResponseAsync(context, HttpStatusCode.InternalServerError, "Ocurrió un error interno. Intente nuevamente.");
            }
        }

        private static async Task WriteResponseAsync(HttpContext context, HttpStatusCode statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var payload = JsonSerializer.Serialize(new { error = message });
            await context.Response.WriteAsync(payload);
        }
    }
}
