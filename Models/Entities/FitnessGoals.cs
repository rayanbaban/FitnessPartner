using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessPartner.Models.Entities
{
	public class FitnessGoals
	{
        [Key]
        public int GoalId { get; set; }

        [ForeignKey("AppUserId")]
        public int AppUserId { get; set; }

        public string GoalDescription { get; set; } = string.Empty;

        public int PrValue { get; set; }

		public virtual AppUser? User { get; set; }

	}
}
