﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public string Email { get; set; }

        [Required]
        [MaxLength(25), MinLength(4)]
        public string Name { get; set; }

        public string AvatarURL { get; set; }

        [Required]
        public float MoneyOnAccount { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreationDate { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastLogInDate { get; set; }

        [Required]
        public ICollection<User> Friends { get; set; }

        [Required]
        public ICollection<GameStats> GamesStats { get; set; }

    }
}
