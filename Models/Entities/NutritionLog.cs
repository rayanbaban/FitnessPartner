﻿using System.ComponentModel.DataAnnotations;
<<<<<<< HEAD
=======
<<<<<<< HEAD
>>>>>>> main
using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations.Schema;


namespace FitnessPartner.Models.Entities
{
    public class NutritionLog
    {
        [Key]
        public int LogId { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string FoodIntake { get; set; } = string.Empty;

    }
}
