using auth_api_ms.DomainModel.Interfaces;
using auth_api_ms.DomainModel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace auth_api_ms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("Register")]
        public async Task<bool> Register([FromBody] RegisterUser registerUser)
        {
            if (registerUser == null)
            {
                return false;
            }
            var res =  await _authService.Register(registerUser);
            return res;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
        {
            if(loginUser == null)
            {
                return BadRequest();
            }
            var res = await _authService.Login(loginUser);
            if(res)
            {
                var jwtString = _authService.GenerateToken(loginUser);
                return Ok(jwtString);
            }
            return BadRequest();
        }
        [HttpPost("AddRole")]
        public async Task<bool> AddRole([FromBody] RegisterUser user)
        {
            if(user!=null)
            {
                var isSuccess = _authService.AssignRole(user.Email, user.Role);
                return isSuccess.Result;
            }
            return false;
        }
    }
}
