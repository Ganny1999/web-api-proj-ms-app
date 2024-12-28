using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace auth_api_ms.DomainModel.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Phone]
        [Required]
        public string PhoneNumber { get; set; }
    }
}
