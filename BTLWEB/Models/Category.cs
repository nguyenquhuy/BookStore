using System.ComponentModel.DataAnnotations;

namespace BTLWEB.Models
{
    public class Category
    {
        [Key]
        public int Id {  get; set; }
        [Required]
        [Display(Name ="Thể loại")]
        public string? Name { get; set; }
    }
}
