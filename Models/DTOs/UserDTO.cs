using System.ComponentModel.DataAnnotations;

namespace FitnessPartner.Models.DTOs
{
	public class UserDTO
	{
		public UserDTO(int userId, int age, string userName, string password,
			string firstName, string lastName, 
			string email, decimal weight, decimal height, DateTime created)
		{
			UserId = userId;
			Age = age;
			UserName = userName;
			Password = password;
			FirstName = firstName;
			LastName = lastName;
			Email = email;
			Weight = weight;
			Height = height;
			Created = created;

		}

		[Required]
		public int UserId { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; }
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
        public DateTime Created { get; set; }

        public bool IsUserAdmin { get; set; }


    }
}
