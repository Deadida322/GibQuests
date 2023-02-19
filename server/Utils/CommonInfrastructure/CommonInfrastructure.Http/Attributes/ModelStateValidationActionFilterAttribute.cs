using CommonInfrastructure.Http.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CommonInfrastructure.Http.Attributes
{
    public class ModelStateValidationActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid) {
                var errors = new List<string>();
                foreach (var elem in modelState.Values)
                {
                    foreach (var error in elem.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                context.Result = new ObjectResult(
                    CommonHttpHelper.BuildErrorResponse(
                        statusCode: System.Net.HttpStatusCode.BadRequest,
                        extErrors: errors,
                        initialError: "Неверные поля модели"
                        ));
            }
            base.OnActionExecuting(context);
        }

    }
}
