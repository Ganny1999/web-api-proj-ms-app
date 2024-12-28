using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace order_api_ms.DomainModel.Models
{
    public class CartItems
    {
        [Key]
        public int CartItemId { get; set; }
        [Required]  
        public int CartID {  get; set; }
        [ForeignKey("CartID")]
        public Cart Cart { get; set; }
        [Required]
        public int ProductID {  get; set; }
        [NotMapped]
        public Product Product { get; set; }
        [Required]
        public int Quantity {  get; set; }

    }
}
