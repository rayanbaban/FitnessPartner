using FitnessPartner.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessPartner.Data
{
    public class FitnessPartnerDbContext : DbContext
    {
        public FitnessPartnerDbContext(DbContextOptions<FitnessPartnerDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<ExerciseSession> ExerciseSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ekstra konfigurasjoner kan legges til her hvis nødvendig
        }
    }
}

