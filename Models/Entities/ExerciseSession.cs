using System.ComponentModel.DataAnnotations;

namespace FitnessPartner.Models.Entities
{
	public class ExerciseSession
	{

		[Required]
		public DateTime Date { get; set; }

		[Required]
		public string ExerciseType { get; set; } = string.Empty;

		[Required]
		public int DurationMinutes { get; set; }

	}
}
