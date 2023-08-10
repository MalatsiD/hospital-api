using Hospital_API.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Hospital_API.Helpers
{
    public static class ModelStateErrorManager
    {
        public static ResponseModelView GetModelStateError(ModelStateDictionary modelState)
        {
            var result = new ResponseModelView();

            var message = string.Join(" | ", modelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage));

            result.StatusCode = StatusCodes.Status400BadRequest;
            result.ErrorMessage = message;
            result.IsSuccessful = false;

            return result;
        }
    }
}
