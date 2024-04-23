using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Mappers
{
	public class UserMapper : IMapper<AppUser, UserDTO>
	{
		public UserDTO MapToDto(AppUser model)
		{
			return new UserDTO(/*model.UserId, */ model.Age, model.UserName, model.PasswordHash, model.FirstName,
				model.LastName, model.Email, model.Weight, model.Height, model.Created);
		}

		public AppUser MapToModel(UserDTO dto)
		{
			var now = DateTime.Now;
			return new AppUser()
			{
//				UserId = dto.UserId,
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
