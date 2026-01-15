using CineSoul.Models;
using System.Collections.Generic;

namespace CineSoul.ViewModels
{
    public class ProfileViewModel
    {
        public List<Movie> RecentMovies { get; set; } = new List<Movie>();
        public List<int> Watchlist { get; set; } = new List<int>();
    }
}