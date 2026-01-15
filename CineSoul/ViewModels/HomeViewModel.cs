using CineSoul.Models; 
using System.Collections.Generic;

namespace CineSoul.ViewModels
{
    public class HomeViewModel
    {
        
        public IEnumerable<Movie> HeroCarouselMovies { get; set; }

        
        public IEnumerable<Movie> TrendingMovies { get; set; }
        public IEnumerable<Movie> NewReleases { get; set; }
        public IEnumerable<Movie> ComingSoon { get; set; }
    }
}