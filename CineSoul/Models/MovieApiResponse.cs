using System.Collections.Generic;
using System.Text.Json.Serialization; // System.Text.Json kullanıyorsak

namespace CineSoul.Models
{
    public class MovieApiResponse
    {
        [JsonPropertyName("page")] // JSON'dan gelen "page" alanını eşler
        public int Page { get; set; }

        [JsonPropertyName("results")]
        // API'dan gelen filmlerin listesidir
        public List<Movie> Results { get; set; }

        [JsonPropertyName("total_pages")]
        public int TotalPages { get; set; }

        [JsonPropertyName("total_results")]
        public int TotalResults { get; set; }
    }
}