using FitnessPartner.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessPartner.Data
{
    public class FitnessPartnerDbContext : DbContext
    {
        public FitnessPartnerDbContext(DbContextOptions<FitnessPartnerDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<>
    }
}
