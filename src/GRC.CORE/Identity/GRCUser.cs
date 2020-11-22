using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GRC.Core.Identity
{
    public class GRCUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Organization { get; set; }

        [Display(Name = "Full Name")]
        [NotMapped]
        public string FullName => $"{Name} {LastName} - {Email}";

    }
}
