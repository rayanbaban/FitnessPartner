using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface INutritionPlansRepository
{
    Task<ICollection<NutritionPlans>> GetMyNutritionPlanAsync(int pageNr, int pageSize);
    Task<NutritionPlans?> UpdateNutritionPlanAsync(NutritionPlans nutritionPlanDTO, int planId);
    Task<NutritionPlans?> DeleteNutritionPlanAsync( int planId);
    Task<NutritionPlans?> CreateNutritionPlanAsync(NutritionPlans nutritionPlan);
    Task<NutritionPlans?> GetNutritionPlanByIdAsync(int planId);
    Task<ICollection<NutritionPlans>> GetPageAsync(int pageNr, int pageSize);
}