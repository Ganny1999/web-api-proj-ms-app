using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace product_api_ms.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var exception=  context.Exception;

            context.Result = new JsonResult(new
            {
                Error = exception,
                details = exception.Message
            })
            {
                StatusCode = 500
            }; 
        }
    }
}
