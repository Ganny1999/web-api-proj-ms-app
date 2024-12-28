using System.ComponentModel.DataAnnotations;

namespace order_place_api_ms.DomainModel.Models
{
    public class Order
    {

        [Key]
        public Guid OrderID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public string PementMethod { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public string PaymentStatus { get; set; }
        [Required]
        public int TotalAmount { get; set; }
    }
}
