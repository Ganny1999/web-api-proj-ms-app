using auth_api_ms.Data;
using auth_api_ms.DomainModel.Interfaces;
using auth_api_ms.DomainModel.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace auth_api_ms.Services
{
    public class AuthServices : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public AuthServices(AppDbContext dbContext, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _dbContext =dbContext;
            _userManager = userManager;
            _configuration = configuration;
        }

        public string GenerateToken(LoginUser loginUser)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("jwt:Key").Value));

            var claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Email,loginUser.Email)
            };

            var signingCredential = new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);

            var tokenToken = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(60),
                issuer: _configuration.GetSection("jwt:Issuer").Value,
                audience:_configuration.GetSection("jwt:Audience").Value,
                signingCredentials: signingCredential,
                claims:claim
                );
            string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenToken);

            return tokenString;
        }

        public async Task<bool> Login(LoginUser loginUser)
        {
            try
            {
                if(loginUser == null)
                {
                    throw new ArgumentNullException(nameof(loginUser), "The login user object cannot be null.");
                }
                if(string.IsNullOrEmpty(loginUser.Email) || string.IsNullOrEmpty(loginUser.Password))
                {
                    return false;
                }

                //  Check if the email and passward is valid or not
                var user = await _userManager.FindByEmailAsync(loginUser.Email);
                if(user == null)
                {
                    return false;
                }
                var isValidUser = await _userManager.CheckPasswordAsync(user, loginUser.Password);

                // JWT

                if (isValidUser)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> Register(RegisterUser registerUser)
        {
            try
            {
                // check register use null
                if(registerUser == null)
                {
                    throw new ArgumentNullException(nameof(registerUser), "The registerUser object cannot be null.");
                }
                if(string.IsNullOrEmpty(registerUser.Email) || string.IsNullOrEmpty(registerUser.Password)) 
                {
                    return false;
                }

                var user = new ApplicationUser()
                {
                    Email = registerUser.Email,
                    PhoneNumber = registerUser.PhoneNumber,
                    Name = registerUser.Name,
                    UserName = registerUser.Email
                };
                
                var result = await _userManager.CreateAsync(user,registerUser.Password);

                return result.Succeeded;
            }
            catch(Exception ex)
            {
                throw ex;   
            }
        }
    }
}
