// Models/CastMember.cs
using System.Text.Json.Serialization;

namespace CineSoul.Models
{
    public class CastMember
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("character")]
        public string Character { get; set; }

        // Sadece ilk 5-10 oyuncuyu göstermek için yeterli olacaktır.
    }
}