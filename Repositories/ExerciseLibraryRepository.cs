using FitnessPartner.Data;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FitnessPartner.Repositories
{
	public class ExerciseLibraryRepository : IExerciseLibraryRepository
	{
		private readonly FitnessPartnerDbContext _dbContext;
		private readonly ILogger<ExerciseLibraryRepository?> _logger;

		public ExerciseLibraryRepository(FitnessPartnerDbContext dbContext, ILogger<ExerciseLibraryRepository?> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public async Task<ExerciseLibrary?> CreateExerciseAsync( ExerciseLibrary exersiceLib)
		{

			try
			{
				var newExerciseEntry = await _dbContext.ExerciseLibrary.AddAsync(exersiceLib);
				_logger.LogDebug("Legger til en ny exercise {@newExercise}", newExerciseEntry.Entity);

				await _dbContext.SaveChangesAsync();
				return newExerciseEntry.Entity;
			}
			catch (DbUpdateException ex)
			{
				_logger.LogError("Feil ved databaseoppdatering: {ErrorMessage}", ex.Message);
				return null;
			}
			catch (Exception ex)
			{
				_logger.LogError("Annen feil ved tillegg av ny exercise: {ErrorMessage}", ex.Message);
				return null;
			}
		}

		public async Task<ExerciseLibrary?> DeleteExerciseAsync(int id)
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

		public async Task<ICollection<ExerciseLibrary>> GetAllExercisesAsync(int pageNr, int pageSize)
		{
			try
			{
				var allExercises = await _dbContext.ExerciseLibrary.ToListAsync();
				return allExercises;
			}
			catch (Exception ex)
			{

				_logger.LogError("Feil ved henting av alle exercises: {ErrorMessage}", ex.Message);
				return null;
			}
			
		}

		public async Task<ExerciseLibrary?> GetExerciseByIdAsync(int id)
		{
			try
			{
				var exerciseId = await _dbContext.ExerciseLibrary.FindAsync(id);
				return exerciseId;

			}
			catch (Exception ex)
			{
				_logger.LogError("Feil ved henting av exercise med ID {exerciseID}:  {ErrorMessage}", id, ex.Message);
				return null;
			}
		}

		public async Task<ExerciseLibrary?> GetExerciseByMuscleNameAsync(string muscleName)
		{
			var muscle = await _dbContext.ExerciseLibrary
				.FirstOrDefaultAsync(x => x.MusclesTrained!.Equals(muscleName));
			return muscle is null ? null : muscle;
		}

		public async Task<ExerciseLibrary?> UpdateExerciseAsync(ExerciseLibrary updatedExercise, int id)
		{
			try
			{
				var existingExercise = await _dbContext.ExerciseLibrary.FindAsync(id);

				if (existingExercise == null)
					if (existingExercise == null)
					{
						_logger.LogWarning("Kunne ikke finne exercise med ID {exerciseId} for oppdatering", id);
						return null;
					}

				existingExercise.ExerciseName = updatedExercise.ExerciseName;
				existingExercise.ExerciseId = updatedExercise.ExerciseId;
				existingExercise.Description = updatedExercise.Description;
				existingExercise.Technique = updatedExercise.Technique;

				await _dbContext.SaveChangesAsync();

				return existingExercise;
			}
			catch (Exception ex)
			{
				_logger.LogError("Feil ved oppdatering av exercise med ID {EventId}: {ErrorMessage}", id, ex.Message);
				return null;
			}
		}
	}
}
