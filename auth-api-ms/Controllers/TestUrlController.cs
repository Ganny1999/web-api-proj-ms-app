using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace auth_api_ms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestUrlController : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles ="ADMIN")]
        public string testUrl()
        {
            return "Yes, its working.";
        }
    }
}
