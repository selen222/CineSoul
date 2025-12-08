// Models/CrewMember.cs
using System.Text.Json.Serialization;

namespace CineSoul.Models
{
    public class CrewMember
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("job")]
        public string Job { get; set; }

        // Yönetmen (Director) veya Senarist (Writer) gibi rolleri filtrelemek için kullanılır.
    }
}