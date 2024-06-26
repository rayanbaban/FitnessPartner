﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitnessPartner.Models.Entities
{
    public class NutritionLog
    {
        [Key]
        public int LogId { get; set; }
        [ForeignKey("AppUserId")]
        public int AppUserId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string FoodIntake { get; set; } = string.Empty;

		public virtual AppUser? User { get; set; }

	}
}
