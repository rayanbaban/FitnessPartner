using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface INutritionLogRepository
{
    Task<ICollection<NutritionLog>> GetMyNutritionLogsAsync(int pageNr, int pageSize);
    Task<NutritionLog?> UpdateNutritionLogAsync(NutritionLog nutritionLogDTO, int logId, int loggedinUser);
    Task<NutritionLog?> DeleteNutritionLogAsync(int userId, int logId);
    Task<NutritionLog?> CreateNutritionLogAsync(NutritionLog nutritionLog, int loggedinUser);
    Task<NutritionLog?> GetNutritionLogByIdAsync(int logId);
    Task<ICollection<NutritionLog?>> GetPageAsync(int pageNr, int pageSize);
}
