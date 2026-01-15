using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CineSoul.Models
{
    
    public class CreditResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        
        [JsonPropertyName("cast")]
        public List<CastMember> Cast { get; set; }

        
        [JsonPropertyName("crew")]
        public List<CrewMember> Crew { get; set; }
    }
}