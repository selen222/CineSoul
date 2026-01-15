// Services/ITmdbService.cs

using CineSoul.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using CineSoul.ApiModels; // TmdbDto'yu kullanıyorsanız

namespace CineSoul.Services
{
    public interface ITmdbService
    {
        // ------------------------------------
        // ANA SAYFA METOTLARI
        // ------------------------------------
        Task<List<Movie>> GetNewReleasesAsync();
        Task<List<Movie>> GetTrendingMoviesAsync();
        Task<List<Movie>> GetPopularMoviesAsync();
        Task<List<Movie>> GetNowPlayingMoviesAsync();
        Task<List<Movie>> GetUpcomingMoviesAsync();

        // ------------------------------------
        // DETAY ve LİSTE METOTLARI
        // ------------------------------------

        // Hata veren metot: Bir filmin tüm detaylarını (oyuncu, yönetmen vb.) çeker
        Task<Movie> GetFullMovieDetailsAsync(int id); // <-- BU SATIR EKLENMELİ/DÜZELTİLMELİ!

        Task<TmdbDto> GetMovieDetailDtoAsync(int id);

        // Arama metodu (varsayılan olarak eklenmeli)
        Task<List<Movie>> SearchMoviesAsync(string query);
    }
}