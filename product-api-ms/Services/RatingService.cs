using Newtonsoft.Json;
using product_api_ms.Services.IExternalServices;

namespace product_api_ms.Services
{
    public class RatingService : IRatingService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public RatingService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<decimal> GetAverageRating(int ProductID)
        {
            var httpClient = _httpClientFactory.CreateClient("Rating");

            var response = await httpClient.GetAsync($"/api/Ratings/AverageRatingProduct/{ProductID}");
            var content = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<decimal>(content);
            return resp;
        }
    }
}
