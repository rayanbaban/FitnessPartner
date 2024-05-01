using FitnessPartner.Data;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessPartner.Repositories
{
    public class NutritionResourcesRepository : INutritionResourcesRepository
    {
        private readonly FitnessPartnerDbContext _dbContext;
        private readonly ILogger<NutritionResourcesRepository> _logger;

        public NutritionResourcesRepository(FitnessPartnerDbContext dbContext, ILogger<NutritionResourcesRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<NutritionResources?> CreateNutritionResourceAsync(NutritionResources nutritionResource)
        {
            try
            {
                var newNutritionResource = await _dbContext.NutritionResources.AddAsync(nutritionResource);
                _logger.LogDebug("Legger til en ny nutritionResource {@nutritionResource}", newNutritionResource.Entity);

                await _dbContext.SaveChangesAsync();
                return newNutritionResource.Entity;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Feil ved databaseoppdatering: {ErrorMessage}", ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Annen feil ved tillegg av ny nutritionResource: {ErrorMessage}", ex.Message);
                return null;
            }
        }

        public async Task<NutritionResources?> DeleteNutritionResourceAsync( int resourceId)
        {
            try
            {
                var resourceToDel = await _dbContext.NutritionResources.FindAsync(resourceId);

                if (resourceToDel == null)
                {
                    _logger.LogWarning("Kunne ikke finne nutritionResource med ID {NutritionResourceID} for sletting.", resourceId);
                    return null;
                }

                _dbContext.NutritionResources.Remove(resourceToDel);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Nutritionesource med Id {nutritionResourceId} ble slettet.", resourceId);
                return resourceToDel;

            }
            catch (Exception ex)
            {
                _logger.LogError("Feil ved sletting av nutritionResource, feilmelding: {error}", ex.Message);
                return null;
            }
        }

        public async Task<NutritionResources?> GetNutritionResourceByIdAsync(int resourceId)
        {
            try
            {
                var nutritionResourceId = await _dbContext.NutritionResources.FindAsync(resourceId);
                return nutritionResourceId;

            }
            catch (Exception ex)
            {
                _logger.LogError("Feil ved henting av nutritionResource med ID {nutritionResourceId}:  {ErrorMessage}", resourceId, ex.Message);
                return null;
            }
        }

        public async Task<ICollection<NutritionResources>?> GetNutritionResourcesAsync(int pageNr, int pageSize)
        {
            try
            {
                var allNutritionResources = await _dbContext.NutritionResources.ToListAsync();
                return allNutritionResources;
            }
            catch (Exception ex)
            {

                _logger.LogError("Feil ved henting av alle nutritionResources: {ErrorMessage}", ex.Message);
                return null;
            }
        }

        public async Task<ICollection<NutritionResources?>?> GetPageAsync(int pageNr, int pageSize)
        {
            try
            {
                var eventsPage = await _dbContext.NutritionResources
                    .Skip((pageNr - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return eventsPage;
            }
            catch (Exception ex)
            {
                _logger.LogError("Feil ved henting av nutitionResources {PageNr} med størrelse {PageSize}: {ErrorMessage}", pageNr, pageSize, ex.Message);
                return null;
            }
        }
        public async Task<NutritionResources?> UpdateNutritionResourceAsync(NutritionResources nutritionResourceDTO, int resourceId)
        {
            try
            {
                var existingNutritionResource = await _dbContext.NutritionResources.FindAsync(resourceId);

                if (existingNutritionResource == null)
                {
                    _logger.LogWarning("Kunne ikke finne nutritionResource med ID {sessionId}", resourceId);
                    return null;
                }
                existingNutritionResource.Content = nutritionResourceDTO.Content;

                await _dbContext.SaveChangesAsync();

                return existingNutritionResource;

            }
            catch (Exception ex)
            {
                _logger.LogError("Kunne ikke oppdatere nutritionLog med nutritionLog ID: {existingNutritionLog}: {ErrorMessage}: ", resourceId, ex.Message);
                return null;
            }
        }
    }
}
