using FitnessPartner.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessPartner.Data
{
    public class FitnessPartnerDbContext : DbContext
    {
<<<<<<< HEAD
        public FitnessPartnerDbContext(DbContextOptions<FitnessPartnerDbContext> options) : base(options)
=======
        public FitnessPartnerDbContext(DbContextOptions<FitnessPartnerDbContext> options)
        : base(options)
>>>>>>> BranchV2
        {
        }

        public DbSet<User> Users { get; set; }
<<<<<<< HEAD
        public DbSet<ExerciseSession> ExerciseSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ekstra konfigurasjoner kan legges til her hvis nødvendig
        }
    }
}

=======
    }
}
>>>>>>> BranchV2
