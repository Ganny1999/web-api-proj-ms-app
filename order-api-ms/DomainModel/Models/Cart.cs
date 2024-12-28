using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace order_api_ms.DomainModel.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }
        [Required]
        public int CustomerID { get; set;}
        [NotMapped]
        public decimal TotalAmount { get;set; }
    }
}
