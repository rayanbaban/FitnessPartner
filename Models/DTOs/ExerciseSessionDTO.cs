namespace FitnessPartner.Models.DTOs
{
	public class ExerciseSessionDTO
	{
		public ExerciseSessionDTO(int userId, DateTime date, string musclesTrained, int durationMinutes, string result, string intensity)
		{
			UserId = userId;
			Date = date;
			MusclesTrained = musclesTrained;
			DurationMinutes = durationMinutes;
			Result = result;
			Intensity = intensity;
		}

		public int UserId { get; set; }
		
		public DateTime Date { get; set; }

		public string MusclesTrained { get; set; } = string.Empty;
		
		public int DurationMinutes { get; set; }
		
		public string Result { get; set; } = string.Empty;

		public string Intensity { get; set; } = string.Empty;


	}
}
