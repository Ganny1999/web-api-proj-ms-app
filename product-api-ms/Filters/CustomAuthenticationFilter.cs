using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace product_api_ms.Filters
{
    public class CustomAuthenticationFilter : IAuthorizationFilter
    {
        private readonly string _role = "USER";
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if(!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
            }
            if(!user.IsInRole(_role))
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
