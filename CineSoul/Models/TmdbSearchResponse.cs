using System.Collections.Generic;

namespace CineSoul.Models
{
    public class TmdbSearchResponse
    {
        public int Page { get; set; }
        public List<TmdbMovieResult> Results { get; set; }
        public int TotalResults { get; set; }
        public int TotalPages { get; set; }
    }
}
