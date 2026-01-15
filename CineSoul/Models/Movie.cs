using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace CineSoul.Models
{
    public class Movie
    {
        [Key]
        [JsonPropertyName("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("original_title")]
        public string OriginalTitle { get; set; }

        [JsonPropertyName("overview")]
        public string Overview { get; set; }

        [JsonPropertyName("release_date")]
        public string ReleaseDate { get; set; } 

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

        [JsonPropertyName("tagline")]
        public string Tagline { get; set; }

        [JsonPropertyName("runtime")]
        public int? Runtime { get; set; }

        [NotMapped]
        [JsonPropertyName("genres")]
        public List<MovieGenre> Genres { get; set; }

        [NotMapped]
        [JsonPropertyName("genre_ids")]
        public List<int> GenreIds { get; set; }

        [JsonPropertyName("adult")]
        public bool Adult { get; set; }

        [JsonPropertyName("video")]
        public bool Video { get; set; }
        public string TrailerUrl { get; set; }
        public string Director { get; set; }
        public string Cast { get; set; }
        public string Screenwriter { get; set; }
        public bool IsTrending { get; set; }
        public bool IsNewRelease { get; set; }

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
                if (!string.IsNullOrEmpty(ReleaseDate) && DateTime.TryParse(ReleaseDate, out DateTime parsedDate))
                {
                    return parsedDate.Year.ToString();
                }
                return "N/A";
            }
        }
    }
    public class MovieGenre
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}