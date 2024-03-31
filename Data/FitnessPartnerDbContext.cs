using FitnessPartner.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessPartner.Data
{
    public class FitnessPartnerDbContext : DbContext
    {
        public FitnessPartnerDbContext(DbContextOptions<FitnessPartnerDbContext> options)
        : base(options)
        {
        }

        public DbSet<ExerciseLibrary> ExerciseLibrary { get; set; }
        public DbSet<ExerciseSession> ExerciseSession { get; set; }
        public DbSet<FitnessGoals> FitnessGoals { get; set; }
        public DbSet<NutritionLog> NutritionLog { get; set; }
        public DbSet<NutritionPlans> NutritionPlans { get; set; }
        public DbSet<NutritionResources> NutritionResources { get; set; }   
        public DbSet<User> Users { get; set; }


	}
}
