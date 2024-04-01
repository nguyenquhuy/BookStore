using System.ComponentModel.DataAnnotations;

namespace BTLWEB.ViewModel
{
    public class AddRoleViewModel
    {
        [Required]
        [Display(Name ="Role")]
        public string? RoleName {  get; set; }
    }
}
