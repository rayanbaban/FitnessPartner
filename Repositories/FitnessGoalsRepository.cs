using FitnessPartner.Data;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FitnessPartner.Repositories
{
	public class FitnessGoalsRepository : IFitnessGoalsRepository
	{
		private readonly ILogger<FitnessGoalsRepository> _logger;
		private readonly FitnessPartnerDbContext _dbContext;

		public FitnessGoalsRepository(ILogger<FitnessGoalsRepository> logger, FitnessPartnerDbContext dbContext)
		{
			_logger = logger;
			_dbContext = dbContext;
		}

		public async Task<FitnessGoals> CreateFitnessGoalAsync(FitnessGoals fitnessGoals)
		{

			try
			{
				var fitnessGoal = await _dbContext.FitnessGoals.AddAsync(fitnessGoals);
				_logger.LogDebug("Legger til et nytt fitness goal {@newGoal}", fitnessGoal.Entity);

				await _dbContext.SaveChangesAsync();
				return fitnessGoal.Entity;
			}
			catch (DbUpdateException ex)
			{
				_logger.LogError("Feil ved databaseoppdatering: {ErrorMessage}", ex.Message);
				return null;
			}
			catch (Exception ex)
			{
				_logger.LogError("Annen feil ved tillegg av nytt fitness goal: {ErrorMessage}", ex.Message);
				return null;
			}
		}

		public async Task<FitnessGoals> DeleteFitnessGoalAsync(int goalId)
		{
			try
			{
				var fitnessGoalToDelete = await _dbContext.FitnessGoals.FindAsync(goalId);

				if (fitnessGoalToDelete == null)
				{
					_logger.LogWarning("Kunne ikke finne fitness goal med ID {goalID} for sletting", goalId);
					return null;
				}

				_dbContext.FitnessGoals.Remove(fitnessGoalToDelete);
				await _dbContext.SaveChangesAsync();

				_logger.LogInformation("Fitness goal med ID {goalID} ble slettet", goalId);
				return fitnessGoalToDelete;
			}
			catch (Exception ex)
			{
				_logger.LogError("Feil ved sletting av fitness goal. Det ble kastet en unntak: {ExceptionMessage}", ex.Message);
				return null;
			}
		}

		public async Task<FitnessGoals> GetFitnessGoalByIdAsync(int goalId)
		{
			try
			{
				var fitnessGoalId = await _dbContext.FitnessGoals.FindAsync(goalId);
				return fitnessGoalId;

			}
			catch (Exception ex)
			{
				_logger.LogError("Feil ved henting av fitness goal med ID {goalID}:  {ErrorMessage}", goalId, ex.Message);
				return null;
			}
		}

		public async Task<ICollection<FitnessGoals>> GetMyFitnessGoalsAsync(int pageNr, int pageSize)
		{
			try
			{
				var allFitnessGoals = await _dbContext.FitnessGoals.ToListAsync();
				return allFitnessGoals;
			}
			catch (Exception ex)
			{
				_logger.LogError("Feil ved henting av alle fitness goals: {ErrorMessage}", ex.Message);
				return null;
			}
		}

		public async Task<ICollection<FitnessGoals>> GetPageAsync(int pageNr, int pageSize)
		{
			try
			{
				var eventsPage = await _dbContext.FitnessGoals
					.Skip((pageNr - 1) * pageSize)
					.Take(pageSize)
					.ToListAsync();

				return eventsPage;
			}
			catch (Exception ex)
			{
				_logger.LogError("Feil ved henting av fitnesside {PageNr} med størrelse {PageSize}: {ErrorMessage}", pageNr, pageSize, ex.Message);
				return null;
			}
		}

		public async Task<FitnessGoals> UpdateFitnessGoalAsync(FitnessGoals fitnessGoals, int goalId)
		{
			try
			{
				var existingFitnessgoal = await _dbContext.FitnessGoals.FindAsync(goalId);

				if (existingFitnessgoal == null)
					if (existingFitnessgoal == null)
					{
						_logger.LogWarning("Kunne ikke finne fitness goal med ID {fitnessGoalId} for oppdatering", goalId);
						return null;
					}

				existingFitnessgoal.PrValue = fitnessGoals.PrValue;
				existingFitnessgoal.GoalDescription = fitnessGoals.GoalDescription;

				await _dbContext.SaveChangesAsync();

				return existingFitnessgoal;
			}
			catch (Exception ex)
			{
				_logger.LogError("Feil ved oppdatering av fitness goal med ID {goalId}: {ErrorMessage}", goalId, ex.Message);
				return null;
			}
		}
	}
}
