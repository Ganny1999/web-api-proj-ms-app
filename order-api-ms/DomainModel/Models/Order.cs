using System.ComponentModel.DataAnnotations.Schema;

namespace order_api_ms.DomainModel.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public Guid OrderIdGuid { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime OrderTime { get; set; }
        public int CustomerID { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
