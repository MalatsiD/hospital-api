using Hospital_API.Exceptions;
using Hospital_API.Helpers;
using Hospital_API.ViewModels;

namespace Hospital_API.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleException(context, ex);
            }
        }

        private static Task HandleException(HttpContext context, Exception ex)
        {
            var result = new ResponseModelView();

            int statusCode = StatusCodes.Status500InternalServerError;

            switch (ex)
            {
                case NotFoundException _:
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                case BadRequestException _:
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
            }

            result.StatusCode = statusCode;
            result.IsSuccessful = false;
            result.ErrorMessage = ex.Message;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return Task.FromResult(result);
        }

    }
}
