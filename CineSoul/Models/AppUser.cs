using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CineSoul.Models
{
    public class AppUser : IdentityUser
    {
        [MaxLength(150)]
        public string DisplayName { get; set; }

        [MaxLength(2000)]
        public string ProfilePicturePath { get; set; } = "/images/default_avatar.png";

        public DateTime JoinedAt { get; set; } = DateTime.Now; // UtcNow yerine yerel saat bazen daha kolay takip edilir

        public ICollection<UserList> Lists { get; set; } = new List<UserList>();
    }
}