using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CineSoul.Models
{
    public enum ListType
    {
        Custom = 0,
        Watchlist = 1,
        Favorites = 2
    }

    public class UserList
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public ListType Type { get; set; } = ListType.Custom;

        [Required]
        public string OwnerId { get; set; }
        public AppUser Owner { get; set; }
        public virtual ICollection<UserListItem> Items { get; set; } = new List<UserListItem>();
    }
}