using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CineSoul.Models
{
    public class Genre
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required] // Bu alanın boş geçilemeyeceğini belirtir
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}