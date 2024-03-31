using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessPartner.Models.Entities
{
	public class NutritionPlans
	{
        [Key]
        public int PlanId { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public string PlanType { get; set; } = string.Empty;
        public string PlanDetails{ get; set; } = string.Empty;

		public virtual User? User { get; set; }

	}
}
