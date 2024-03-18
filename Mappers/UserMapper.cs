using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Mappers
{
	public class UserMapper : IMapper<User, UserDTO>
	{
		public UserDTO MapToDto(User model)
		{
			return new UserDTO(model.UserId, model.UserName, model.FirstName, model.LastName, model.Email,model.Weight ,model.Height,
				model.Created, model.Age);
		}

		public User MapToModel(UserDTO dto)
		{
			var now = DateTime.Now;
			return new User()
			{1
				UserId = dto.UserId,
				UserName = dto.UserName,
				FirstName = dto.FirstName,
				LastName = dto.LastName,
				Weight = dto.Weight,
				Height = dto.Height,
				Created = dto.Created,
				Age = dto.Age,
			};
		}
	}
}
