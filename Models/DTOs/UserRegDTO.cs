using System.ComponentModel.DataAnnotations;

namespace FitnessPartner.Models.DTOs
{
	public class UserRegDTO
	{

		[Required]
		public string UserName { get; init; } = string.Empty;

		[Required]
		public string FirstName { get; init; } = string.Empty;

		[Required]
		public string LastName { get; init; } = string.Empty;


		[Required]
		public string Gender { get; init; } = string.Empty;
		
		[Required]
		public int Age { get; init; }

		
		[Required]
		public decimal Weight { get; init; }

		[Required]
		public decimal Height { get; init; }

		[Required]
		public string Email { get; init; } = string.Empty;

		[Required]
		public string Password { get; init; } = string.Empty;
		
	}
}
