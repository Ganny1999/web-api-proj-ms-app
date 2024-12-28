using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace auth_api_ms.DomainModel.Models
{
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
