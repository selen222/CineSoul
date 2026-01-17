using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CineSoul.Models;
using System.Linq; 
using System;
using CineSoul.ApiModels;

namespace CineSoul.Services
{
    
    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _httpClient;

        
        private readonly string _apiKey = "0e4c342979a7673b6cba91526f68d643";
        private readonly string _baseUrl = "https://api.themoviedb.org/3";
        private readonly string _language = "tr-TR";

        public TmdbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Movie>> GetTrendingMoviesAsync()
        {
            var url = $"{_baseUrl}/trending/movie/week?api_key={_apiKey}&language={_language}";
            var response = await _httpClient.GetFromJsonAsync<MovieApiResponse>(url);
            return response?.Results ?? new List<Movie>();
        }

        
        public async Task<List<Movie>> GetPopularMoviesAsync()
        {
            var url = $"{_baseUrl}/movie/popular?api_key={_apiKey}&language={_language}";
            var response = await _httpClient.GetFromJsonAsync<MovieApiResponse>(url);
            return response?.Results ?? new List<Movie>();
        }

        
        public async Task<List<Movie>> GetNowPlayingMoviesAsync()
        {
            var url = $"{_baseUrl}/movie/now_playing?api_key={_apiKey}&language={_language}";
            var response = await _httpClient.GetFromJsonAsync<MovieApiResponse>(url);
            return response?.Results ?? new List<Movie>();
        }

        public async Task<List<Movie>> GetNewReleasesAsync()
        {
            
            return await GetNowPlayingMoviesAsync();

           
        }

       
        public async Task<List<Movie>> GetUpcomingMoviesAsync()
        {
            var url = $"{_baseUrl}/movie/upcoming?api_key={_apiKey}&language={_language}";
            var response = await _httpClient.GetFromJsonAsync<MovieApiResponse>(url);
            return response?.Results ?? new List<Movie>();
        }

        public async Task<Movie> GetBaseMovieDetailsAsync(int id)
        {
            var url = $"{_baseUrl}/movie/{id}?api_key={_apiKey}&language={_language}";
            return await _httpClient.GetFromJsonAsync<Movie>(url);
        }

        
        public async Task<CreditResponse> GetMovieCreditsAsync(int id)
        {
            var url = $"{_baseUrl}/movie/{id}/credits?api_key={_apiKey}";
            return await _httpClient.GetFromJsonAsync<CreditResponse>(url);
        }


        public async Task<Movie> GetFullMovieDetailsAsync(int id)
        {
            
            var movie = await GetBaseMovieDetailsAsync(id);
            if (movie == null) return null;

            
            var credits = await GetMovieCreditsAsync(id);
            if (credits != null)
            {
                
                movie.Director = credits.Crew.FirstOrDefault(c => c.Job == "Director")?.Name ?? "Bilinmiyor";

                
                movie.Screenwriter = credits.Crew.FirstOrDefault(c => c.Job == "Screenplay" || c.Job == "Writer")?.Name ?? "Bilinmiyor";

               
                movie.Cast = string.Join(", ", credits.Cast.Take(10).Select(c => c.Name));
            }

            
            var videoUrl = $"{_baseUrl}/movie/{id}/videos?api_key={_apiKey}";
            var videoResponse = await _httpClient.GetFromJsonAsync<VideoApiResponse>(videoUrl);

           
            var trailer = videoResponse?.Results?.FirstOrDefault(v => v.Type == "Trailer" && v.Site == "YouTube");
            if (trailer != null)
            {
                movie.TrailerUrl = $"https://www.youtube.com/embed/{trailer.Key}";
            }

            return movie;
        }

        
        public async Task<List<Movie>> SearchMoviesAsync(string query)
        {
            var url = $"{_baseUrl}/search/movie?api_key={_apiKey}&language={_language}&query={query}";

            var response = await _httpClient.GetFromJsonAsync<MovieApiResponse>(url);
            return response?.Results ?? new List<Movie>();
        }

        
        public Task<TmdbDto> GetMovieDetailDtoAsync(int id)
        {
            
            return Task.FromResult<TmdbDto>(null);
        }

        
        public async Task<Movie> GetMovieDetailsAsync(int id)
        {
            
            var url = $"{_baseUrl}/movie/{id}?api_key={_apiKey}&language=tr-TR&append_to_response=videos,credits";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var movie = await response.Content.ReadFromJsonAsync<Movie>();
                
                return movie;
            }

            return null;
        }

        public async Task<List<Movie>> GetMultipleMoviesAsync(List<int> movieIds)
        {
            var movies = new List<Movie>();
            if (movieIds == null || !movieIds.Any()) return movies;

            foreach (var id in movieIds)
            {
                
                var movie = await GetMovieDetailsAsync(id);
                if (movie != null)
                {
                    movies.Add(movie);
                }
            }
            return movies;
        }
    }
}