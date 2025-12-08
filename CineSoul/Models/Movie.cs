using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Veritabanı ayarları için gerekli
using System.Text.Json.Serialization;

namespace CineSoul.Models
{
    public class Movie
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("original_title")]
        public string OriginalTitle { get; set; }

        [JsonPropertyName("overview")]
        public string Overview { get; set; }

        [JsonPropertyName("release_date")]
        public DateTime? ReleaseDate { get; set; } 

        [JsonPropertyName("vote_average")]
        public double VoteAverage { get; set; }

        [JsonPropertyName("vote_count")]
        public int VoteCount { get; set; }

        [JsonPropertyName("poster_path")]
        public string PosterPath { get; set; }

        [JsonPropertyName("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonPropertyName("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonPropertyName("popularity")]
        public double Popularity { get; set; }

        // --- DİKKAT: SQL Hatasını önlemek için NotMapped ekledik ---
        // Bu alan API'den gelir ama Veritabanına sütun olarak kaydedilmez.
        [NotMapped]
        [JsonPropertyName("genre_ids")]
        public List<int> GenreIds { get; set; }

        [JsonPropertyName("adult")]
        public bool Adult { get; set; }

        [JsonPropertyName("video")]
        public bool Video { get; set; }

        // ==========================================================
        // PROJE İSTERLERİ İÇİN EKLEDİĞİMİZ YENİ ALANLAR
        // ==========================================================

        // 1. Youtube Trailer Linki (Popup için)
        public string TrailerUrl { get; set; }

        // 2. Yönetmen ve Oyuncular (Detay sayfası için)
        public string Director { get; set; }
        public string Cast { get; set; } // Örn: "Brad Pitt, Edward Norton"

        // 3. Carousel Kategorileri (Trendler, Yeni Çıkanlar vb.)
        // Veritabanına kaydederken bunları biz işaretleyeceğiz.
        public bool IsTrending { get; set; }
        public bool IsNewRelease { get; set; }

        // ==========================================================
        // YARDIMCI PROPERTY'LER (Database'e kaydedilmez, UI içindir)
        // ==========================================================

        [JsonIgnore]
        public string FullPosterUrl =>
            string.IsNullOrEmpty(PosterPath) ? "https://via.placeholder.com/500x750?text=No+Poster" : $"https://image.tmdb.org/t/p/w500{PosterPath}";

        [JsonIgnore]
        public string FullBackdropUrl =>
            string.IsNullOrEmpty(BackdropPath) ? "https://via.placeholder.com/1280x720?text=No+Image" : $"https://image.tmdb.org/t/p/original{BackdropPath}";

        [JsonIgnore]
        public string Year
        {
            get
            {
                if (ReleaseDate.HasValue)
                {
                    return ReleaseDate.Value.Year.ToString();
                }
                return "N/A";
            }
        }
    }
}