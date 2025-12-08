using CineSoul.Models; // Movie modelinizin bulunduğu namespace
using System.Collections.Generic;

namespace CineSoul.ViewModels
{
    public class HomeViewModel
    {
        // 1. Sayfanın En Üstünde yer alacak BÜYÜK Carousel (Manşet) Filmleri
        public IEnumerable<Movie> HeroCarouselMovies { get; set; }

        // 2. Alt kısımda yer alacak YATAY Küçük Karusel Listeleri
        public IEnumerable<Movie> TrendingMovies { get; set; }
        public IEnumerable<Movie> NewReleases { get; set; }
        public IEnumerable<Movie> ComingSoon { get; set; }
    }
}