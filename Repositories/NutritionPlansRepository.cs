using FitnessPartner.Data;
using FitnessPartner.Models.Entities;
using FitnessPartner.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitnessPartner.Repositories;

public class NutritionPlansRepository : INutritionPlansRepository
{
    private readonly FitnessPartnerDbContext _dbContext;
    private readonly ILogger<NutritionPlansRepository> _logger;

    public NutritionPlansRepository(FitnessPartnerDbContext dbContext, ILogger<NutritionPlansRepository> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<NutritionPlans?> CreateNutritionPlanAsync(NutritionPlans nutritionPlan, int loggedinUser)
    {
        try
        {
            var newNutritionPlan = await _dbContext.NutritionPlans.AddAsync(nutritionPlan);
            _logger.LogDebug("Legger til en ny nutritionPlan {@nutritionPlan}", newNutritionPlan.Entity);

            await _dbContext.SaveChangesAsync();
            return newNutritionPlan.Entity;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError("Feil ved databaseoppdatering: {ErrorMessage}", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError("Annen feil ved tillegg av ny nutritionPlan: {ErrorMessage}", ex.Message);
            return null;
        }
    }

    public async Task<NutritionPlans?> DeleteNutritionPlanAsync(int userId, int planId)
    {
        try
        {

            var planToDel = await _dbContext.NutritionPlans.FindAsync(planId);

            if (planToDel == null)
            {
                _logger.LogWarning("Kunne ikke finne nutritionPlan med ID {NutritionPlanID} for sletting.", planId);
                return null;
            }

            _dbContext.NutritionPlans.Remove(planToDel);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("NutritionPlan med Id {nutritionPlanId} ble slettet.", planId);
            return planToDel;

        }
        catch (Exception ex)
        {
            _logger.LogError("Feil ved sletting av nutritionPlan, feilmelding: {error}", ex.Message);
            return null;
        }
    }

    public async Task<ICollection<NutritionPlans>?> GetMyNutritionPlanAsync(int pageNr, int pageSize)
    {
        try
        {
            var allNutritionPlans = await _dbContext.NutritionPlans.ToListAsync();
            return allNutritionPlans;
        }
        catch (Exception ex)
        {

            _logger.LogError("Feil ved henting av alle nutritionPlans: {ErrorMessage}", ex.Message);
            return null;
        }
    }

    public async Task<NutritionPlans?> GetNutritionPlanByIdAsync(int planId)
    {
        try
        {
            var nutritionPlanId = await _dbContext.NutritionPlans.FindAsync(planId);
            return nutritionPlanId;

        }
        catch (Exception ex)
        {
            _logger.LogError("Feil ved henting av nutritionPlan med ID {nutritionPlanId}:  {ErrorMessage}", planId, ex.Message);
            return null;
        }
    }

    public async Task<ICollection<NutritionPlans>?> GetPageAsync(int pageNr, int pageSize)
    {
        try
        {
            var eventsPage = await _dbContext.NutritionPlans
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

    public async Task<NutritionPlans?> UpdateNutritionPlanAsync(NutritionPlans nutritionPlanDTO, int planId, int loggedinUser)
    {
        try
        {
            var existingNutritionPlan = await _dbContext.NutritionPlans.FindAsync(planId);

            if (existingNutritionPlan == null)
            {
                _logger.LogWarning("Kunne ikke finne nutritionPlan med ID {nutritionPlanId}", planId);
                return null;
            }
            existingNutritionPlan.PlanDetails = nutritionPlanDTO.PlanDetails;

            await _dbContext.SaveChangesAsync();

            return existingNutritionPlan;

        }
        catch (Exception ex)
        {
            _logger.LogError("Kunne ikke oppdatere nutritionPlan med nutritionPlan ID: {existingNutritionPlan}: {ErrorMessage}: ", planId, ex.Message);
            return null;
        }
    }
}
