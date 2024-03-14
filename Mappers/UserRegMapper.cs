using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Mappers
{
	public class UserRegMapper : IMapper<User, UserRegDTO>
	{
		public UserRegDTO MapToDto(User model)
		{
			throw new NotImplementedException();
		}

		public User MapToModel(UserRegDTO dto)
		{
			var dtNow = DateTime.Now;
			var dateOfBirth = dtNow.AddYears(-dto.Age);
			return new User()
			{
				Email = dto.Email,
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Weight = dto.Weight,
				Height = dto.Height,
				IsAdminUser = false,
				Created = dtNow,
				Updated = dtNow,
				Age = dto.Age
			};
		}
	}
}
