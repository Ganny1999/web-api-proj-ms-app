using System.ComponentModel.DataAnnotations;

namespace order_place_api_ms.DomainModel.Models
{
    public class Cart
    {
        [Key]
        public int CartID { get; set; }
        [Required]
        public int CustomerID { get; set;}
        public decimal TotalAmount { get;set; }
    }
}
