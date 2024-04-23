using System.ComponentModel.DataAnnotations;

namespace FitnessPartner.Models.DTOs
{
	public class UpdatePermissionDTO
	{

		[Required(ErrorMessage = "Username is required")]
		public string UserName { get; init; } = string.Empty;
	}
}
