using CineSoul.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CineSoul.ApiModels; 

namespace CineSoul.Services
{
    public interface ITmdbService
    {
        Task<List<Movie>> GetNewReleasesAsync();
        Task<List<Movie>> GetTrendingMoviesAsync();
        Task<List<Movie>> GetPopularMoviesAsync();
        Task<List<Movie>> GetNowPlayingMoviesAsync();
        Task<List<Movie>> GetUpcomingMoviesAsync();

       
        Task<Movie> GetFullMovieDetailsAsync(int id);

        
        Task<TmdbDto> GetMovieDetailDtoAsync(int id);

        
        Task<List<Movie>> SearchMoviesAsync(string query);
    }
}