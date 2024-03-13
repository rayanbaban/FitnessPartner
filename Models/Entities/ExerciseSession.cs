using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessPartner.Models.Entities
{
    [Table("ExerciseSessions")]
    public class ExerciseSession
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(100)] // Juster maksimal lengde basert på dine krav
        public string ExerciseType { get; set; } = string.Empty;

        [Required]
        public int DurationMinutes { get; set; }
    }
}

