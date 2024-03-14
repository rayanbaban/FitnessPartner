using System.ComponentModel.DataAnnotations;

namespace FitnessPartner.Models.DTOs
{
	public class UserDTO
	{
		 [Key]
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
		public string Password { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
        public decimal Weight { get; set; } 
        [Required]
        public decimal Height { get; set; } 

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Updated { get; set; }

    }
}
