using System.ComponentModel.DataAnnotations;

namespace FitnessPartner.Models.Entities
{
    public class NutritionResources
    {
        [Key]
        public int ResourceId { get; set; }
        [Required]
        public string ResourceTitle { get; set; } = string.Empty;
        [Required]
        public string ResourceType { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
