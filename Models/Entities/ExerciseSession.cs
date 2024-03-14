using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessPartner.Models.Entities
{
	public class ExerciseSession
	{

        [ForeignKey("UserId")]
        public int UserId { get; set; }
        [Required]
		public DateTime Date { get; set; }

		[Required]
		public string MusclesTrained { get; set; } = string.Empty;

		[Required]
		public int DurationMinutes { get; set; }

		[Required]
		public string Result { get; set; } = string.Empty;
		[Required]
		public string Intensity { get; set; } = string.Empty;


    }
}
