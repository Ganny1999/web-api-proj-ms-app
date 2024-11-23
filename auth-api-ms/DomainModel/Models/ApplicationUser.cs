using Microsoft.AspNetCore.Identity;

namespace auth_api_ms.DomainModel.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}

