using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CineSoul.Models
{
    // TMDB API'dan /credits endpoint'inden gelen yanıtı tutar.
    public class CreditResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        // Oyuncu kadrosunu tutar (Cast)
        [JsonPropertyName("cast")]
        public List<CastMember> Cast { get; set; }

        // Ekibi tutar (Yönetmen vb. için kullanılır)
        [JsonPropertyName("crew")]
        public List<CrewMember> Crew { get; set; }
    }
}