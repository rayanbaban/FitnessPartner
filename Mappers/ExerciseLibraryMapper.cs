using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Mappers
{
	public class ExerciseLibraryMapper : IMapper<ExerciseLibrary, ExerciseLibraryDTO>
	{
		public ExerciseLibraryDTO MapToDto(ExerciseLibrary model)
		{
			return new ExerciseLibraryDTO(
				model.ExerciseId,
				model.ExerciseName,
				model.Description,
				model.Technique);
		}

		public ExerciseLibrary MapToModel(ExerciseLibraryDTO dto)
		{
			return new ExerciseLibrary()
			{
				ExerciseId = dto.ExerciseId,
				ExerciseName = dto.ExerciseName,
				Description = dto.Description,
				Technique = dto.Technique,
			};
		}
	}
}
