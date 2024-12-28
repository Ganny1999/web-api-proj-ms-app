namespace product_api_ms.Services.IExternalServices
{
    public interface IRatingService
    {
        Task<decimal> GetAverageRating(int ProductID);
    }
}
