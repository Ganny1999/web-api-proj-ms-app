using System.ComponentModel.DataAnnotations;

namespace rating_api_ms.DomainModel.Models
{
    public class Rating
    {
        [Required]
        [Key]
        public int RatingId { get; set; }
        [Range(1,5)]
        [Required]
        public int RatingValue { get; set; }
        [Required]
        public Guid UserID { get; set; }
        [Required]
        public int ProductID { get; set; }
        public string? comment { get; set; }

    }
}
