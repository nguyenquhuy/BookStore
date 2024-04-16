using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BTLWEB.Models
{
    public class BookCategory
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }
        public Book Book { get; set; } // Navigation property for Book

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; } // Navigation property for Category
    }
}
