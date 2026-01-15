using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineSoul.Models
{
    public class Rating
    {
        public int Id { get; set; }

        [Range(1, 10)]
        public int Value { get; set; }

        public DateTime RatedAt { get; set; } = DateTime.Now;

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

        public int MovieId { get; set; }
        public string? MovieTitle { get; set; }
        public string? PosterPath { get; set; }
    }
}