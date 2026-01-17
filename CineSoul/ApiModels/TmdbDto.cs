using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace CineSoul.ApiModels
{
    public class TmdbDto
    {
        [JsonPropertyName("adult")]
        public bool Adult { get; set; }

        [JsonPropertyName("backdrop_path")]
        public string Backdrop_Path { get; set; }

        [JsonPropertyName("budget")]
        public int Budget { get; set; }

        [JsonPropertyName("genres")]
        public List<TmdbGenre> Genres { get; set; }

        [JsonPropertyName("homepage")]
        public string Homepage { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("imdb_id")]
        public string Imdb_Id { get; set; }

        [JsonPropertyName("original_language")]
        public string Original_Language { get; set; }

        [JsonPropertyName("original_title")]
        public string Original_Title { get; set; }

        [JsonPropertyName("overview")]
        public string Overview { get; set; }

        [JsonPropertyName("popularity")]
        public double Popularity { get; set; }

        [JsonPropertyName("poster_path")]
        public string Poster_Path { get; set; }

        [JsonPropertyName("production_companies")]
        public List<ProductionCompany> Production_Companies { get; set; }

        [JsonPropertyName("production_countries")]
        public List<ProductionCountry> Production_Countries { get; set; }

        [JsonPropertyName("release_date")]
        public string Release_Date { get; set; }

        [JsonPropertyName("revenue")]
        public long Revenue { get; set; }

        [JsonPropertyName("runtime")]
        public int Runtime { get; set; }

        [JsonPropertyName("spoken_languages")]
        public List<SpokenLanguage> Spoken_Languages { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("tagline")]
        public string Tagline { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("video")]
        public bool Video { get; set; }

        [JsonPropertyName("vote_average")]
        public double Vote_Average { get; set; }

        [JsonPropertyName("vote_count")]
        public int Vote_Count { get; set; }



        [JsonPropertyName("videos")]
        public TmdbVideos Videos { get; set; } 

        [JsonPropertyName("credits")]
        public TmdbCredits Credits { get; set; }
    }


    public class TmdbGenre
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class ProductionCompany
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("logo_path")]
        public string Logo_Path { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("origin_country")]
        public string Origin_Country { get; set; }
    }

    public class ProductionCountry
    {
        [JsonPropertyName("iso_3166_1")]
        public string Iso_3166_1 { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class SpokenLanguage
    {
        [JsonPropertyName("iso_639_1")]
        public string Iso_639_1 { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class TmdbCredits
    {


        [JsonPropertyName("cast")]
        public List<TmdbCast> Cast { get; set; } 

        [JsonPropertyName("crew")]
        public List<TmdbCrew> Crew { get; set; }
    }

    public class TmdbCast
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

    }

    public class TmdbCrew
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("job")]
        public string Job { get; set; }
    }

    public class TmdbVideos
    {


        [JsonPropertyName("results")]
        public List<TmdbVideoResult> Results { get; set; }
    }

    public class TmdbVideoResult
    {
        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("site")]
        public string Site { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } 
    }
}