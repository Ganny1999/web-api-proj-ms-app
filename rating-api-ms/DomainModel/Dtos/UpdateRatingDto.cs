namespace rating_api_ms.DomainModel.Dtos
{
    public class UpdateRatingDto
    {
        public int RatingValue { get; set; }
        public Guid UserID { get; set; }
        public int ProductID { get; set; }
    }
}
