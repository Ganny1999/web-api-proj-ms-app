using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace product_api_ms.Filters
{
    public class CustomActionFilter : IActionFilter
    {
        private readonly ILogger<CustomActionFilter> _logger;
        public CustomActionFilter(ILogger<CustomActionFilter> logger)
        {
            _logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("Action filter started its execution.");
            if(context.ModelState.IsValid)
            {
                _logger.LogInformation("Model state is valid and we can move further.");
            }
            else
            {
                _logger.LogInformation("Model state is not valid, kindly provide the valid model data.");
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("Action filter completed its execution.");
        }
    }
}
