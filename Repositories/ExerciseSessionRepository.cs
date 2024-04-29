using FitnessPartner.Data;
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

        public async Task<ExerciseSession?> AddSessionAsync(ExerciseSession session)
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
            catch (Exception ex)
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

        public async Task<ICollection<ExerciseSession?>?> GetAllSessionsAsync(int pageNr, int pageSize)
        {
            try
            {
                var ExerciseSessions = await _dbContext.ExerciseSession.ToListAsync();
                return ExerciseSessions;
            }
            catch (Exception ex)
            {
                _logger.LogError("Feil ved henting av alle ExerciseSessions: {ErrorMessage}", ex.Message);
                return null;
            }
        }

        public async Task<IEnumerable<ExerciseSession?>> GetPageAsync(int pageNr, int pageSize)
        {
            var totCount = _dbContext.ExerciseSession.Count();
            var totPages = (int)Math.Ceiling((double)totCount / pageSize);

            return await _dbContext.ExerciseSession
                .OrderBy(x => x.SessionId)
                .Skip((pageNr - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<ExerciseSession?> GetSessionsByIdAsync(int id)
        {
            try
            {
				var sessionId = await _dbContext.ExerciseSession.FindAsync(id);
				return sessionId;
            }
            catch (Exception ex)
            {
                _logger.LogError("Feil ved henting av session med ID {sessionId}: {ErrorMessage}", id, ex.Message);
                return null;
            }
        }

        public async Task<ExerciseSession?> UpdateSessionsAsync(ExerciseSession session, int id)
        {
            try
            {
                var existingSession = await _dbContext.ExerciseSession.FindAsync(id);

                if (existingSession == null)
                {
                    _logger.LogWarning("Kunne ikke finne session med ID {sessionId}", id);
                    return null;
                }
                existingSession.DurationMinutes = session.DurationMinutes;
                existingSession.Result = session.Result;
                existingSession.MusclesTrained = session.MusclesTrained;
                existingSession.Intensity = session.Intensity;

                await _dbContext.SaveChangesAsync();

                return existingSession;

            }
            catch (Exception ex)
            {
                _logger.LogError("Kunne ikke oppdatere session med session ID: {existingses}: {ErrorMessage}: ", id, ex.Message);
                return null;
            }
        }
    }
}
