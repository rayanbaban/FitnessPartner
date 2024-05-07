using FitnessPartner.Data;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessPartner.Repositories
{
    public class NutritionLogRepository : INutritionLogRepository
    {
        private readonly FitnessPartnerDbContext _dbContext;
        private readonly ILogger<NutritionLogRepository> _logger;

        public NutritionLogRepository(FitnessPartnerDbContext dbContext, ILogger<NutritionLogRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<NutritionLog?> CreateNutritionLogAsync(NutritionLog nutritionLog)
        {
            try
            {
                var newNutritionLog = await _dbContext.NutritionLog.AddAsync(nutritionLog);
                _logger.LogDebug("Legger til en ny nutritionLog {@nutritionLog}", newNutritionLog.Entity);

                await _dbContext.SaveChangesAsync();
                return newNutritionLog.Entity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Feil ved databaseoppdatering: {ErrorMessage}", ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Annen feil ved tillegg av ny nutritionLog: {ErrorMessage}", ex.Message);
                return null;
            }
        }

        public async Task<NutritionLog?> DeleteNutritionLogAsync(int logId)
        {
            try
            {

                var logToDel = await _dbContext.NutritionLog.FindAsync(logId);

                if (logToDel == null)
                {
                    _logger.LogWarning("Kunne ikke finne nutritionLog med ID {NutritionLogID} for sletting.", logId);
                    return null;
                }

                _dbContext.NutritionLog.Remove(logToDel);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("NutritionLog med Id {nutritionLogId} ble slettet.", logId);
                return logToDel;

            }
            catch (Exception ex)
            {
                _logger.LogError("Feil ved sletting av nutritionLog, feilmelding: {error}", ex.Message);
                return null;
            }
        }

        public async Task<ICollection<NutritionLog>?> GetAllNutritionLogsAsync(int pageNr, int pageSize)
        {
            try
            {
                var allNutritionLogs = await _dbContext.NutritionLog.ToListAsync();
                return allNutritionLogs;
            }
            catch (Exception ex)
            {

                _logger.LogError("Feil ved henting av alle nutritionLogs: {ErrorMessage}", ex.Message);
                return null;
            }
        }

		public async Task<ICollection<NutritionLog>> GetMyNutritionLogsAsync(string userId, int pageNr, int pageSize)
		{
			int skip = (pageNr - 1) * pageSize;

			var logs = await _dbContext.NutritionLog
				.Where(session => session.User.Id == userId)
				.Skip(skip)
				.Take(pageSize)
				.ToListAsync();

			return logs;
		}

		public async Task<NutritionLog?> GetNutritionLogByIdAsync(int logId)
        {
            try
            {
                var nutritionLogId = await _dbContext.NutritionLog.FindAsync(logId);
                return nutritionLogId;

            }
            catch (Exception ex)
            {
                _logger.LogError("Feil ved henting av nutritionLog med ID {nutritionLogID}:  {ErrorMessage}", logId, ex.Message);
                return null;
            }
        }

        public async Task<ICollection<NutritionLog?>?> GetPageAsync(int pageNr, int pageSize)
        {
            try
            {
                var eventsPage = await _dbContext.NutritionLog
                    .Skip((pageNr - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return eventsPage;
            }
            catch (Exception ex)
            {
                _logger.LogError("Feil ved henting av nutitionLog {PageNr} med størrelse {PageSize}: {ErrorMessage}", pageNr, pageSize, ex.Message);
                return null;
            }
        }

        public async Task<NutritionLog?> UpdateNutritionLogAsync(NutritionLog nutritionLogDTO, int logId)
        {
            try
            {
                var existingNutritionLog = await _dbContext.NutritionLog.FindAsync(logId);

                if (existingNutritionLog == null)
                {
                    _logger.LogWarning("Kunne ikke finne nutritionLog med ID {sessionId}", logId);
                    return null;
                }
                existingNutritionLog.FoodIntake = nutritionLogDTO.FoodIntake;

                await _dbContext.SaveChangesAsync();

                return existingNutritionLog;

            }
            catch (Exception ex)
            {
                _logger.LogError("Kunne ikke oppdatere nutritionLog med nutritionLog ID: {existingNutritionLog}: {ErrorMessage}: ", logId, ex.Message);
                return null;
            }
        }
    }
}
