﻿using FitnessPartner.Data;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessPartner.Repositories
{
    public class ExerciseSessionRepository : IExersiceSessionRepository
	{

		private readonly FitnessPartnerDbContext _dbContext;
		private readonly ILogger<ExerciseSessionRepository> _logger;

		public ExerciseSessionRepository(FitnessPartnerDbContext dbContext, ILogger<ExerciseSessionRepository> logger)
		{
			_dbContext = dbContext;
			_logger = logger;
		}

		public async Task<ExerciseSession?> AddSessionAsync(ExerciseSession session, int id)
		{
			try
			{
				var newSession = await _dbContext.AddAsync(session);
				_logger.LogDebug("Legger til en ny session {@nysession}", newSession.Entity);

				await _dbContext.SaveChangesAsync();
				return newSession.Entity;
			}
			catch (DbUpdateException ex)
			{

				_logger.LogError("Feil ved database oppdatering {message}", ex.Message);
				return null;
			}
			catch(Exception ex )
			{
				_logger.LogError("Feilet ved opprettelse av ny session {message}", ex.Message);
				return null;
			}
		}


		public async Task<ExerciseSession?> DeleteSessionsAsync(int id)
		{
			try
			{
				var sessionsToDelete = await _dbContext.ExerciseSession.FindAsync(id);

				if (sessionsToDelete == null)
				{
					_logger.LogWarning("Kunne ikke finne session {sessionId}", id);
					return null;
				}

				_dbContext.ExerciseSession.Remove(sessionsToDelete);
				await _dbContext.SaveChangesAsync();

				_logger.LogInformation("Session med Id {sessionID} ble slettet", id);
				return sessionsToDelete;
			}
			catch (Exception ex)
			{
				_logger.LogError("Feil ved sletting av session. Det ble kastet en unntak: {ExceptionMessage}", ex.Message);
				return null;

			}
		}

		public Task<ExerciseSession?> GetSessionsAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<ExerciseSession?> UpdateSessionsAsync(ExerciseSession session, int id)
		{
			throw new NotImplementedException();
		}
	}
}
