namespace FitnessPartner.Models.DTOs
{
    public class NutritionLogDTO
    {
		public NutritionLogDTO(int logId, int userId, DateTime date, string foodIntake)
		{
			LogId = logId;
			UserId = userId;
			Date = date;
			FoodIntake = foodIntake;
		}

		public int LogId { get; set; }

        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string FoodIntake { get; set; } = string.Empty;
    }
}
