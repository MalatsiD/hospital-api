using Hospital_API.Exceptions;
using Hospital_API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hospital_API.ActionFilters
{
    public class ValidationFilterAttribute : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.ModelState.IsValid)
            {
                HandleModelState(context.ModelState, context);
                return;
            }

            await next();
        }

        private static void HandleModelState(ModelStateDictionary modelState, ActionExecutingContext context)
        {
            var result = new ResponseModelView();

            int statusCode = StatusCodes.Status400BadRequest;

            result.StatusCode = statusCode;
            result.IsSuccessful = false;
            result.ErrorMessage = string.Join(" | ", modelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));

            context.Result = new BadRequestObjectResult(result);
        }
    }
}
