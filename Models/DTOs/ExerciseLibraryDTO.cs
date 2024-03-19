
namespace FitnessPartner.Models.DTOs
{
	public class ExerciseLibraryDTO
	{
		public ExerciseLibraryDTO(int exerciseId, string exerciseName, string description, string technique)
		{
			ExerciseId = exerciseId;
			ExerciseName = exerciseName;
			Description = description;
			Technique = technique;
		}

		public int ExerciseId { get; set; }
		
		public string ExerciseName { get; set; } = string.Empty;
		
		public string Description { get; set; } = string.Empty;
		
		public string Technique { get; set; } = string.Empty;

	}
}
