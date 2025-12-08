using System.Collections.Generic;

namespace CineSoul.Models
{
    public class TmdbMovieResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Original_Title { get; set; }
        public string Overview { get; set; }
        public string Poster_Path { get; set; }
        public string Backdrop_Path { get; set; }

        public string Release_Date { get; set; }
        public double Vote_Average { get; set; }
        public int Vote_Count { get; set; }

        public List<int> Genre_Ids { get; set; }
        public bool Adult { get; set; }
        public double Popularity { get; set; }
    }
}
