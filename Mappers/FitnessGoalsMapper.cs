using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Mappers
{
	public class FitnessGoalsMapper : IMapper<FitnessGoals, FitnessGoalsDTO>
	{
		public FitnessGoalsDTO MapToDto(FitnessGoals model)
		{
			return new FitnessGoalsDTO(
				model.GoalId,
				model.AppUserId,
				model.GoalDescription,
				model.PrValue);
		}

		public FitnessGoals MapToModel(FitnessGoalsDTO dto)
		{
			return new FitnessGoals()
			{
				AppUserId = dto.UserId,
				GoalId = dto.GoalId,
				GoalDescription = dto.GoalDescription,
				PrValue = dto.PrValue
			};
		}
	}
}
