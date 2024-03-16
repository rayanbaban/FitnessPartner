using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessPartner.Models.DTOs
{
	public class FitnessGoalsDTO
	{
		public FitnessGoalsDTO(int goalId, int userId, string goalDescription, int prValue)
		{
			GoalId = goalId;
			UserId = userId;
			GoalDescription = goalDescription;
			PrValue = prValue;
		}

		public int GoalId { get; set; }

		public int UserId { get; set; }

		public string GoalDescription { get; set; } = string.Empty;

		public int PrValue { get; set; }

	}
}
