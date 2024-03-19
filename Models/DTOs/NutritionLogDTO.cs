namespace FitnessPartner.Models.DTOs
{
    public class NutritionLogDTO
    {
        private int _logId;
        private int _userId;
        private DateTime _date;
        private string _foodIntake;

        public NutritionLogDTO(int logId, int userId, DateTime date, string foodIntake)
        {
            _logId = logId;
            _userId = userId;
            _date = date;
            _foodIntake = foodIntake;
        }

        public int LogId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string FoodIntake { get; set; } = string.Empty;
    }
}
