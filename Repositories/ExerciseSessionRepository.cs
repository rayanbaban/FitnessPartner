using FitnessPartner.Data;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using FitnessPartner.Repositories.Interfaces.IUserRepository;

namespace FitnessPartner.Repositories
{
    public class ExerciseSessionRepository : IExersiceSessionRepository
	{

		private readonly FitnessPartnerDbContext _dbContext;
		private readonly ILogger<ExerciseSessionRepository> _logger;
		public Task<ExerciseSession?> CreateSessionAsync(ExerciseSession session, int id)
		{
			throw new NotImplementedException();
		}

		public Task<ExerciseSession?> DeleteSessionsAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ExerciseSession?> UpdateSessionsAsync(ExerciseSession session, int id)
		{
			throw new NotImplementedException();
		}
	}
}
