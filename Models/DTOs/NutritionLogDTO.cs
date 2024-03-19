namespace FitnessPartner.Models.DTOs
{
<<<<<<< HEAD
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> main
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
<<<<<<< HEAD
=======
=======
	public class NutritionLogDTO
	{
=======
    public class NutritionLogDTO
    {
>>>>>>> BranchV2
		public NutritionLogDTO(int logId, int userId, DateTime date, string foodIntake)
		{
			LogId = logId;
			UserId = userId;
			Date = date;
			FoodIntake = foodIntake;
		}

		public int LogId { get; set; }
<<<<<<< HEAD
>>>>>>> 55c9c0e1477aa35f17748aec687e9dd873e00a69
>>>>>>> main
=======

>>>>>>> BranchV2
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public string FoodIntake { get; set; } = string.Empty;
    }
}
