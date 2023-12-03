﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebAPI.Models
{
    public class Game
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(256),MinLength(1)]
        public string Title { get; set; } = null!;

        public string? LogoURL { get; set; }

        [Required]
        [Range(0.0, 9999.0)]
        public float PriceUsd { get; set; }

        [Required]
        public DateTime PublishDate { get; set; }

        [Range(0, 9999)]
        public int AchievementsAmount { get; set; }

        [Required]
        public int DeveloperID { get; set; }

        [JsonIgnore]
        public Developer? Developer { get; set; }

        [JsonIgnore]
        public ICollection<Review>? Reviews { get; set; } = new List<Review>();
    }
}
