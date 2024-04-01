﻿using System.ComponentModel.DataAnnotations;

namespace BTLWEB.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Language { get; set; }
        [Required]
        [MaxLength(13)]
        public string? ISBN { get; set; }
        [Required, DataType(DataType.Date)]
        [Display(Name = "Date Published")]
        public DateTime DatePublished { get; set; }
        [Required, DataType(DataType.Currency)]
        public int Price { get; set; }
        [Required]
        public string? Author { get; set; }
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }
    }
}
