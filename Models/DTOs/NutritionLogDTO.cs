namespace FitnessPartner.Models.DTOs
{
<<<<<<< HEAD
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
=======
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
>>>>>>> 55c9c0e1477aa35f17748aec687e9dd873e00a69
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string FoodIntake { get; set; } = string.Empty;
    }
}
