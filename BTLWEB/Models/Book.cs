using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [NotMapped]
        public int[] CategoryIds { get; set; }
        // Navigation property for BookCategories
        public ICollection<BookCategory> BookCategories { get; set; }

        [NotMapped]
        public MultiSelectList? MultiCategoryList { get; set; }
        [NotMapped]
        public List<int>? Categories { get; set; }
        [NotMapped]
        public string? CategoryName { get; set; }

    }
}
