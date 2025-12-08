using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CineSoul.Models;
using System.Linq; // OrderBy ve FirstOrDefault için eklendi
using System; // NotImplementedException için eklendi
using CineSoul.ApiModels; // Eğer DTO'ları kullanıyorsanız gereklidir

namespace CineSoul.Services
{
    // Sınıfın ITmdbService arayüzünü uyguladığını belirtiyoruz
    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _httpClient;

        // Sizin verdiğiniz API Key
        private readonly string _apiKey = "0e4c342979a7673b6cba91526f68d643";
        private readonly string _baseUrl = "https://api.themoviedb.org/3";
        private readonly string _language = "tr-TR"; // Veriler Türkçe gelsin

        public TmdbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // ==========================================
        // 1. ANA SAYFA İÇİN GEREKLİ METODLAR
        // ==========================================

        // Trend Filmleri Getir (Haftalık)
        public async Task<List<Movie>> GetTrendingMoviesAsync()
        {
            var url = $"{_baseUrl}/trending/movie/week?api_key={_apiKey}&language={_language}";
            var response = await _httpClient.GetFromJsonAsync<MovieApiResponse>(url);
            return response?.Results ?? new List<Movie>();
        }

        // Popüler Filmleri Getir
        public async Task<List<Movie>> GetPopularMoviesAsync()
        {
            var url = $"{_baseUrl}/movie/popular?api_key={_apiKey}&language={_language}";
            var response = await _httpClient.GetFromJsonAsync<MovieApiResponse>(url);
            return response?.Results ?? new List<Movie>();
        }

        // Vizyondaki (Yeni) Filmleri Getir
        public async Task<List<Movie>> GetNowPlayingMoviesAsync()
        {
            var url = $"{_baseUrl}/movie/now_playing?api_key={_apiKey}&language={_language}";
            var response = await _httpClient.GetFromJsonAsync<MovieApiResponse>(url);
            return response?.Results ?? new List<Movie>();
        }

        public async Task<List<Movie>> GetNewReleasesAsync()
        {
            // Yeni Çıkanlar, genellikle Vizyondakiler veya Popüler Filmler ile aynı kaynaktır.
            // TMDB'de tam bir "New Releases" endpoint'i olmadığı için, GetNowPlaying'i çağırabiliriz.
            return await GetNowPlayingMoviesAsync();

            // Veya Popüler Filmleri döndürebilirsiniz:
            // return await GetPopularMoviesAsync();
        }

        // YAKINDAKİ (UPCOMING) FİLMLERİ GETİR - HomeViewModel için eklendi
        public async Task<List<Movie>> GetUpcomingMoviesAsync()
        {
            var url = $"{_baseUrl}/movie/upcoming?api_key={_apiKey}&language={_language}";
            var response = await _httpClient.GetFromJsonAsync<MovieApiResponse>(url);
            return response?.Results ?? new List<Movie>();
        }


        // ==========================================
        // 2. DETAY VE ARAMA İŞLEMLERİ
        // ==========================================

        // Film Detayını Getir (Temel Movie objesi döner)
        public async Task<Movie> GetBaseMovieDetailsAsync(int id)
        {
            var url = $"{_baseUrl}/movie/{id}?api_key={_apiKey}&language={_language}";
            return await _httpClient.GetFromJsonAsync<Movie>(url);
        }

        // Ekip Bilgisini Getir
        public async Task<CreditResponse> GetMovieCreditsAsync(int id)
        {
            var url = $"{_baseUrl}/movie/{id}/credits?api_key={_apiKey}";
            return await _httpClient.GetFromJsonAsync<CreditResponse>(url);
        }

        // Zenginleştirilmiş Detay Modeli Dön
        public async Task<Movie> GetFullMovieDetailsAsync(int id)
        {
            // 1. Temel Detayları Çek
            var movie = await GetBaseMovieDetailsAsync(id);

            if (movie == null)
            {
                return null;
            }

            // 2. Ekip (Credits) Bilgisini Çek
            var credits = await GetMovieCreditsAsync(id);

            if (credits != null)
            {
                // 3. Director (Yönetmen) Bilgisini Bul ve Movie objesine ekle
                var director = credits.Crew.FirstOrDefault(c => c.Job == "Director");
                movie.Director = director?.Name ?? "Bilinmiyor";

                // 4. Cast (Oyuncular) Bilgisini Birleştir ve Movie objesine ekle
                // İlk 5 oyuncuyu alalım
                // NOT: CastMember modelinizde Order property'si varsa sıralama yapılabilir.
                movie.Cast = string.Join(", ", credits.Cast
                                                .Take(5)
                                                .Select(c => c.Name));
            }

            return movie;
        }

        // Film Arama (Search)
        public async Task<List<Movie>> SearchMoviesAsync(string query)
        {
            var url = $"{_baseUrl}/search/movie?api_key={_apiKey}&language={_language}&query={query}";

            var response = await _httpClient.GetFromJsonAsync<MovieApiResponse>(url);
            return response?.Results ?? new List<Movie>();
        }

        // ITmdbService'de tanımlı olması gereken metot, sizin DTO yapınız nedeniyle burada eksik olabilir.
        // Eğer Details.cshtml TmdbDto değil, Movie modelini bekliyorsa bu metot gereklidir:
        public Task<TmdbDto> GetMovieDetailDtoAsync(int id)
        {
            // Bu metot, detay sayfasının beklediği karmaşık DTO'yu döndürmelidir.
            // Şu anki TmdbService kodunuz Movie döndürdüğü için bu metot ya kaldırılmalı
            // ya da Movie objesini TmdbDto'ya dönüştürmelidir.
            // Önceki adımlardaki varsayımları sürdürerek, DTO metodunu basitçe null döndürüyorum:
            return Task.FromResult<TmdbDto>(null);
        }
    }
}