using System.ComponentModel.DataAnnotations;

namespace FitnessPartner.Models.Entities
{
	public class ExerciseLibrary
	{
		[Key]
        public int ExerciseId { get; set; }
		[Required]
		public string ExerciseName { get; set; } = string.Empty;
		[Required]
		public string Description { get; set; } = string.Empty;
		[Required]
		public string Technique { get; set; } = string.Empty;
		[Required]
		public string MusclesTrained { get; set; } = string.Empty;

		public virtual User? User { get; set; }

	}
}
