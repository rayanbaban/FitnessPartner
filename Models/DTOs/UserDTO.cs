using System.ComponentModel.DataAnnotations;

namespace FitnessPartner.Models.DTOs
{
	public class UserDTO
	{
		public UserDTO(int userId,
			string userName, 
			string firstName, string lastName, decimal weight, decimal height, string email, DateTime created)
		{
			UserId = userId;
			UserName = userName;
			FirstName = firstName;
			LastName = lastName;
			Weight = weight;
			Height = height;
			Email = email;
			Created = created;
		}

        [Required]
		public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;

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


    }
}
