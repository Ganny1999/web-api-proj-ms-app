using System.ComponentModel.DataAnnotations;

namespace order_place_api_ms.DomainModel.Models
{
    public class OrderItems
    {
        [Key]
        public int OrderItemID { get; set; }
        [Required]
        public Guid OrderID { get; set; }
        [Required]
        public int ProductID { get; set; }
        [Required]
        public int Quntity { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public int Price { get; set; }
    }
}
