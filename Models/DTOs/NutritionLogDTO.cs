namespace FitnessPartner.Models.DTOs
{
	public class NutritionLogDTO
	{
        public int LogId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string FoodIntake { get; set; } = string.Empty;
    }
}
