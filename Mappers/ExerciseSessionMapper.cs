using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Mappers
{
	public class ExerciseSessionMapper : IMapper<ExerciseSession, ExerciseSessionDTO>
	{
		public ExerciseSessionDTO MapToDto(ExerciseSession model)
		{
			return new ExerciseSessionDTO(
				model.UserId,
				model.Date,
				model.MusclesTrained,
				model.DurationMinutes,
				model.Result,
				model.Intensity
				);
		}

		public ExerciseSession MapToModel(ExerciseSessionDTO dto)
		{
			return new ExerciseSession()
			{
				UserId = dto.UserId,
				Date = dto.Date,
				MusclesTrained = dto.MusclesTrained,
				DurationMinutes = dto.DurationMinutes,
				Result = dto.Result,
				Intensity = dto.Intensity
			};
		}
	}
}
