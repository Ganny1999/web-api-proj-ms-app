using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace order_api_ms.DomainModel.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }
        [Required]
        public int OrderID { get; set; }
        [ForeignKey("OrderID")]
        public Order Order {  get; set; }
        [Required]
        public int productID { get; set; }
        [Required]
        public int ProductQuantity { get; set; }
        [Required]
        public int ProductPrice { get; set; }
    }
}
