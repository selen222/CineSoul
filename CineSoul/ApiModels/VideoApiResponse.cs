using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CineSoul.ApiModels
{
    public class VideoApiResponse
    {
        [JsonPropertyName("results")]
        public List<VideoResult> Results { get; set; }
    }

    public class VideoResult
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("site")]
        public string Site { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}