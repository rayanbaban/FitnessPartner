namespace FitnessPartner.Models.DTOs
{
	public class UserRegDTO
	{

		public string FirstName { get; init; } = string.Empty;
		public string UserName { get; init; } = string.Empty;
		public string LastName { get; init; } = string.Empty;
		public string Gender { get; init; } = string.Empty;
		public int Age { get; init; } 
		public decimal Height { get; init; } 
		public decimal Weight { get; init; } 
		public string Email { get; init; } = string.Empty;
		public string Password { get; init; } = string.Empty;

		
	}
}
