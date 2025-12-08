// Models/UserList.cs (Geliştirilmiş Tam Kodu)

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CineSoul.Models
{
    // Listelerin sabit tiplerini tanımlarız (İzleme listesi, Favoriler)
    public enum ListType
    {
        Custom = 0, // Kullanıcının oluşturduğu listeler
        Watchlist = 1, // İzleme Listesi
        Favorites = 2  // Favori Listesi
    }

    public class UserList
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        // Yeni: Listenin tipini tutar
        [Required]
        public ListType Type { get; set; } = ListType.Custom;

        // İlişkiler
        [Required]
        public string OwnerId { get; set; }
        public AppUser Owner { get; set; } // Navigasyon Property

        // TMDB film ID'lerini tutar (JSON olarak saklanacak)
        public List<int> MovieIds { get; set; } = new List<int>();
    }
}