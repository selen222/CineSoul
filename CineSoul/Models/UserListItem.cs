using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CineSoul.Models
{
    public class UserListItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserListId { get; set; }

        [ForeignKey("UserListId")]
        public UserList UserList { get; set; }

        [Required]
        public int MovieId { get; set; }
        public string MovieTitle { get; set; }
        public string PosterPath { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.Now;
    }
}