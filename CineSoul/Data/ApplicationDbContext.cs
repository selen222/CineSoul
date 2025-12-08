using CineSoul.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion; // ValueConverter için
using System.Text.Json; // JSON serileştirme için
using System.Linq; // SequenceEqual için
using Microsoft.EntityFrameworkCore.ChangeTracking; // ValueComparer için

namespace CineSoul.Data
{
    // IdentityDbContext'ten miras alıyoruz ki kullanıcı tabloları otomatik gelsin
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Mevcut DbSet'ler
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }

        // YENİ EKLENEN DbSet: Kullanıcı Listeleri
        public DbSet<UserList> UserLists { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Identity tablolarının oluşturulması için temel sınıfı çağır
            base.OnModelCreating(builder);

            // --- UserList için Value Converter Tanımlaması ---

            // 1. Value Converter: List<int> tipini veritabanında JSON string olarak saklar.
            var listConverter = new ValueConverter<List<int>, string>(
                // Kayıt (Write): Listeyi JSON'a dönüştür
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                // Okuma (Read): JSON'dan Listeye dönüştür
                v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions)null)
            );

            // 2. Value Comparer: EF Core'un listeyi doğru şekilde karşılaştırmasına yardımcı olur.
            // Bu, listedeki değişikliklerin algılanmasını sağlar.
            var listComparer = new ValueComparer<List<int>>(
                (c1, c2) => c1.SequenceEqual(c2), // Eşitlik kontrolü
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())), // Hash kodu oluşturma
                c => c.ToList() // Kopyalama (Snapshot)
            );

            // MovieIds property'sine dönüştürücüleri uygula
            builder.Entity<UserList>()
                .Property(l => l.MovieIds)
                .HasConversion(listConverter, listComparer);

            // AppUser ile UserList arasındaki ilişkiyi tanımla (Bire Çok İlişki)
            builder.Entity<UserList>()
                .HasOne(l => l.Owner) // Bir liste bir sahibe sahiptir
                .WithMany(u => u.Lists) // Bir sahibin birden çok listesi vardır
                .HasForeignKey(l => l.OwnerId) // OwnerId yabancı anahtardır
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse, listeler de silinsin
        }
    }
}