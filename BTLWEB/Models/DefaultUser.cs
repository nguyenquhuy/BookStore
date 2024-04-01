using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace BTLWEB.Models
{
    public class DefaultUser : IdentityUser
    {
        [PersonalData]
        public string Name { get; set; }
        [PersonalData]
        public string Adress { get; set; }
        [PersonalData]
        [DataType(DataType.Date)]
        public DateTime UserCreationDate { get; set; } = DateTime.Now;
    }
}
