using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CineSoul.Models
{
    public enum ListType
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public ListType Type { get; set; } = ListType.Custom;

        [Required]
        public string OwnerId { get; set; }
        public AppUser Owner { get; set; } // Navigasyon Property

        // TMDB film ID'lerini tutar (JSON olarak saklanacak)
        public List<int> MovieIds { get; set; } = new List<int>();
    }
}