using Microsoft.EntityFrameworkCore;

namespace FitnessPartner.Data.Migrations
{
	public class FitnessPartnerDbContext : DbContext
	{
		public FitnessPartnerDbContext(DbContextOptions<FitnessPartnerDbContext> options)
		: base(options)
		{
		}
	}
}
