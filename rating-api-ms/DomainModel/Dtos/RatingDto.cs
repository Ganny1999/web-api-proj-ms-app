using System.ComponentModel.DataAnnotations;

namespace rating_api_ms.DomainModel.Dtos
{
    public class RatingDto
    {
        public int RatingId { get; set; }
        public int RatingValue { get; set; }
        public Guid UserID { get; set; }
        public int ProductID { get; set; }
        public string? comment { get; set; }

    }
}
