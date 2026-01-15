using CineSoul.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CineSoul.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<UserList> UserLists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // --- UserList için Value Converter Tanımlaması ---

            var listConverter = new ValueConverter<List<int>, string>(
                // Kayıt (Write): Listeyi JSON'a dönüştür
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                // Okuma (Read): JSON'dan Listeye dönüştür
                v => JsonSerializer.Deserialize<List<int>>(v, (JsonSerializerOptions)null)
            );

            builder.Entity<UserList>()
                .Property(l => l.MovieIds)
                .HasConversion(listConverter, listComparer);

            builder.Entity<UserList>()
                .HasOne(l => l.Owner) // Bir liste bir sahibe sahiptir
                .WithMany(u => u.Lists) // Bir sahibin birden çok listesi vardır
                .HasForeignKey(l => l.OwnerId) // OwnerId yabancı anahtardır
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse, listeler de silinsin
        }
    }
}