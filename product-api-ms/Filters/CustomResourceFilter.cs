using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace product_api_ms.Filters
{
    public class CustomResourceFilter : IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            Console.WriteLine("Resource Filter: pre-action filter logic started.");
            if (context.HttpContext.Request.Method  != "GET") 
            {
                context.Result = new ContentResult()
                {
                    Content = "Request blocked by Resource Filter due to unsupported method.",
                    StatusCode = 405,
                };
            }
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            Console.WriteLine("Resource Filter: Post-action logic executed.");
            string response = context.HttpContext.Response.StatusCode.ToString();
            Console.WriteLine(response);
            //context.HttpContext.Response.Cookies.Append("response",response);
        }
    }
}
