using FitnessPartner.Models.DTOs;

namespace FitnessPartner.Services.Interfaces
{
    public interface INutritionPlanService
    {
        Task<ICollection<NutritionPlansDTO>> GetAllNutritionPlanAsync(int pageNr, int pageSize);
		Task<ICollection<NutritionPlansDTO>> GetMyNutritionPlansAsync(string userId, int pageNr, int pageSize);

		Task<NutritionPlansDTO?> UpdateNutritionPlanAsync(NutritionPlansDTO nutritionPlansDTO, int planId);
        Task<NutritionPlansDTO?> DeleteNutritionPlanAsync(int planId);
        Task<NutritionPlansDTO?> CreateNutritionPlanAsync(NutritionPlansDTO nutritionPlan);
        Task<NutritionPlansDTO?> GetNutritionPlanByIdAsync(int planId);
        Task<ICollection<NutritionPlansDTO>> GetPageAsync(int pageNr, int pageSize);
    }
}
