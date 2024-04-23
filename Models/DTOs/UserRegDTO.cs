using System.ComponentModel.DataAnnotations;

namespace FitnessPartner.Models.DTOs
{
	public class UserRegDTO
	{

		[Required(ErrorMessage = "Username is required")]
		public string UserName { get; init; } = string.Empty;

		[Required(ErrorMessage = "Firstname is required")]
		public string FirstName { get; init; } = string.Empty;

		[Required(ErrorMessage = "Lastname is required")]
		public string LastName { get; init; } = string.Empty;


		[Required(ErrorMessage = "Age is required")]
		public string Gender { get; init; } = string.Empty;
		
		[Required(ErrorMessage = "Age is required")]
		public int Age { get; init; }
		[Required(ErrorMessage = "Weight is required")]
		public decimal Weight { get; init; }

		[Required(ErrorMessage = "Height is required")]
		public decimal Height { get; init; }

		[Required(ErrorMessage = "Email is required")]
		public string Email { get; init; } = string.Empty;

		[Required(ErrorMessage = "Password is required")]
		public string Password { get; init; } = string.Empty;
		
	}
}
