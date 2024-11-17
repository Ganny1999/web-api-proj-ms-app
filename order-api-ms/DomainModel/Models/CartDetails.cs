namespace order_api_ms.DomainModel.Models
{
    public class CartDetails
    {
        public Cart Cart { get; set; }
        public IEnumerable<CartItems>? CartItems { get; set; }
        public double totalAmount { get; set; }
    }
}
