using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface INutritionPlansRepository
{
    Task<ICollection<NutritionPlans>> GetMyNutritionPlanAsync(int pageNr, int pageSize);
    Task<NutritionPlans?> UpdateNutritionPlanAsync(NutritionPlans nutritionPlanDTO, int planId, int loggedinUser);
    Task<NutritionPlans?> DeleteNutritionPlanAsync(int userId, int planId);
    Task<NutritionPlans?> CreateNutritionPlanAsync(NutritionPlans nutritionPlan, int loggedinUser);
    Task<NutritionPlans?> GetNutritionPlanByIdAsync(int planId);
}