using System.ComponentModel.DataAnnotations;

namespace order_place_api_ms.DomainModel.Dtos
{
    public class ProductDto
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Discription { get; set; }
        public int Price { get; set; }
        public int rating { get; set; }
        public string Originated { get; set; }
    }
}
