using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessPartner.Models.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;

		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
        public decimal Weight { get; set; } 
        [Required]
        public decimal Height { get; set; } 
        [Required]
        public int Age { get; set; }

        [Required]
		public string PasswordHash { get; set; } = string.Empty;
        [Required]
        public string Salt { get; set; } = string.Empty;

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Updated { get; set; }

        [Required]
        public bool IsAdminUser { get; set; } 
    }
}
