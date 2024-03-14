using System.ComponentModel.DataAnnotations;

namespace FitnessPartner.Models.DTOs
{
	public class UserDTO
	{
		 [Key]
        public int UserId { get; set; }

        [Required]
        [MinLength(3), MaxLength(30)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        [Required]
		public string Password { get; set; } = string.Empty;
        [Required]
        public string Weight { get; set; } = string.Empty;
        [Required]
        public string Height { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
		public DateTime DateOfBirth { get; set; }

        public string Salt { get; set; } = string.Empty;

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Updated { get; set; }

        [Required]
        public bool IsAdminUser { get; set; }

    }
}
