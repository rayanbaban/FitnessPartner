using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Mappers
{
	public class UserMapper : IMapper<AppUser, UserDTO>
	{
		public UserDTO MapToDto(AppUser model)
		{
			return new UserDTO(model.AppUserId,  model.Age, model.AppUserName, model.PasswordHashed, model.FirstName,
				model.LastName, model.AppUserEmail, model.Weight, model.Height, model.Created);
		}

		public AppUser MapToModel(UserDTO dto)
		{
			var now = DateTime.Now;
			return new AppUser()
			{
				AppUserId = dto.AppUserId,
				UserName = dto.UserName,
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Weight = dto.Weight,
				Height = dto.Height,
				Created = now,
				Age = dto.Age,
			};
		}
	}
}
