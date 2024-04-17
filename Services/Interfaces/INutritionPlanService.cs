using FitnessPartner.Models.DTOs;

namespace FitnessPartner.Services.Interfaces
{
    public interface INutritionPlanService
    {
        Task<ICollection<NutritionPlansDTO>> GetMyNutritionPlanAsync(int pageNr, int pageSize);
        Task<NutritionPlansDTO?> UpdateNutritionPlanAsync(NutritionPlansDTO nutritionPlansDTO, int planId, int loggedinUser);
        Task<NutritionPlansDTO?> DeleteNutritionPlanAsync(int userId, int planId);
        Task<NutritionPlansDTO?> CreateNutritionPlanAsync(NutritionPlansDTO nutritionPlan, int loggedinUser);
        Task<NutritionPlansDTO?> GetNutritionPlanByIdAsync(int planId);
        Task<ICollection<NutritionPlansDTO>> GetPageAsync(int pageNr, int pageSize);
    }
}
