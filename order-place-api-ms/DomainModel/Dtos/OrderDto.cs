using System.ComponentModel.DataAnnotations;

namespace order_place_api_ms.DomainModel.Dtos
{
    public class OrderDto
    {
        public Guid OrderID { get; set; }
        public int CustomerID { get; set; }
        public string PementMethod { get; set; }
        public string PaymentStatus { get; set; }
        public int TotalAmount { get; set; }
    }
}
