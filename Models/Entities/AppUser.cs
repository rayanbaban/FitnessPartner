using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessPartner.Models.Entities
{
    public class AppUser : IdentityUser
    {
        [Key]
		public int AppUserId { get; set; }

        [Required]
        public string AppUserName { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;

		[EmailAddress]
		public string AppUserEmail { get; set; } = string.Empty;

		[Required]
        public decimal Weight { get; set; } 
        [Required]
        public decimal Height { get; set; } 
        [Required]
        public int Age { get; set; }

        [Required]

		public string PasswordHashed { get; set; } = string.Empty;

        [Required]
        public string Salt { get; set; } = string.Empty;

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public bool IsAdminUser { get; set; }


	}
}
