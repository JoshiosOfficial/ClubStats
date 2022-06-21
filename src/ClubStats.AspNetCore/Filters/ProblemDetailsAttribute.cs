using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ClubStats.AspNetCore.Filters;

public class ProblemDetailsAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ModelState.IsValid ||
            context.Controller is not ControllerBase controllerBase)
        {
            return;
        }

        var problemDetails = new ValidationProblemDetails(context.ModelState);
        context.Result = controllerBase.BadRequest(problemDetails);
    }
}