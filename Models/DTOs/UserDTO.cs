using System.ComponentModel.DataAnnotations;

namespace FitnessPartner.Models.DTOs
{
	public class UserDTO
	{
		[Required]
        public  string UserName { get; set; }
		[Required]
        public  string Password { get; set; }
    }
}
