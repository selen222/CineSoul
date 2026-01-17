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
        public DbSet<UserListItem> UserListItems { get; set; }
        public DbSet<WatchHistory> WatchHistories { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserListItem>()
                .HasOne(ui => ui.UserList)
                .WithMany(l => l.Items)
                .HasForeignKey(ui => ui.UserListId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<WatchHistory>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId);

            builder.Entity<UserList>()
                .HasOne(l => l.Owner)
                .WithMany(u => u.Lists)
                .HasForeignKey(l => l.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}