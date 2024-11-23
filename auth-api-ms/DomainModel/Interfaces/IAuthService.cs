using auth_api_ms.DomainModel.Models;

namespace auth_api_ms.DomainModel.Interfaces
{
    public interface IAuthService
    {
        string GenerateToken(LoginUser loginUser);
        Task<bool> Login(LoginUser loginUser);
        Task<bool> Register(RegisterUser registerUser);
    }
}
