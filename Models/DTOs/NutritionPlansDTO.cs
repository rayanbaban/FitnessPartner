namespace FitnessPartner.Models.DTOs
{
	public class NutritionPlansDTO
	{
		public NutritionPlansDTO(int planId, int userId, string planType, string planDetails)
		{
			PlanId = planId;
			UserId = userId;
			PlanType = planType;
			PlanDetails = planDetails;
		}

		public int PlanId { get; set; }
		
		public int UserId { get; set; }
		public string PlanType { get; set; } = string.Empty;
		public string PlanDetails { get; set; } = string.Empty;


	}
}
