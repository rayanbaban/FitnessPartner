using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessPartner.Models.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3), MaxLength(30)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)] // Kan justere maksimal lengde basert på våre krav.
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)] // Kan justere maksimal lengde basert på våre krav.
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Updated { get; set; }

        [Required]
        public bool IsAdminUser { get; set; }
    }
}
