using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Mappers
{
	public class UserRegMapper : IMapper<AppUser, UserRegDTO>
	{
		public UserRegDTO MapToDto(AppUser model)
		{
			throw new NotImplementedException();
		}

		public AppUser MapToModel(UserRegDTO dto)
		{
			var dtNow = DateTime.Now;
			var dateOfBirth = dtNow.AddYears(-dto.Age);
			return new AppUser()
			{
				Email = dto.Email,
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				UserName = dto.UserName,
				Weight = dto.Weight,
				Height = dto.Height,
				Created = dtNow,
				Age = dto.Age,
				IsAdminUser = false
			};
		}
	}
}
