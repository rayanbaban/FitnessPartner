using FitnessPartner.Data;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;

namespace FitnessPartner.Repositories
{
	public class ExerciseLibraryRepository : IExerciseLibrary
	{
		private readonly FitnessPartnerDbContext _dbContext;
		private readonly ILogger<ExerciseLibraryRepository> _logger;
		public async Task<ExerciseLibrary> DeleteExerciseAsync(int id)
		{
			try
			{
				
				var exToDel = await _dbContext.ExerciseLibrary.FindAsync(id);
				
				if (exToDel == null)
				{
					_logger.LogWarning("Kunne ikke finne exercise med ID {ExerciseID} for sletting.", id);
					return null;
				}

				_dbContext.ExerciseLibrary.Remove(exToDel);
				await _dbContext.SaveChangesAsync();

				_logger.LogInformation("Exercise med Id {exerciseId} ble slettet.", id);
				return exToDel;
				
			}
			catch (Exception ex)
			{
				_logger.LogError("Feil ved sletting av exercise, feilmelding: {error}", ex.Message);
				return null;
			}
		}

		public async Task<ExerciseLibrary> GetExercisetByMuscleNameAsync(string muscleName)
		{
			throw new NotImplementedException();
		}

		public Task<ExerciseLibrary> UpdateExerciseAsync(int id)
		{
			throw new NotImplementedException();
		}
	}
}
