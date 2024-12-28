using System.ComponentModel.DataAnnotations;

namespace order_place_api_ms.DomainModel.Models
{
    public class CartItems
    {
        [Key]
        public int CartItemId { get; set; }
        [Required]  
        public int CartID {  get; set; }
        public Cart Cart { get; set; }
        public int ProductID {  get; set; }
        public Product Product { get; set; }
        public int Quantity {  get; set; }

    }
}
