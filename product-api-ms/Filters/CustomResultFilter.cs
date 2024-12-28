using Microsoft.AspNetCore.Mvc.Filters;

namespace product_api_ms.Filters
{
    public class CustomResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Request.Headers.Add("x-header", "my-header");
            context.HttpContext.Response.StatusCode=201;
            context.HttpContext.Response.Cookies.Append("result-cookies", "my-resuolt", new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(1),
                HttpOnly = true,
                Path = "/api/ProductV",
                Secure = true,
                SameSite=SameSiteMode.Strict,

            }) ;

        }
        public void OnResultExecuted(ResultExecutedContext context)
        { 
        }
    }
}
