using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces;

public interface INutritionLogRepository
{
    Task<ICollection<NutritionLog>> GetAllNutritionLogsAsync(int pageNr, int pageSize);
    Task<ICollection<NutritionLog>> GetMyNutritionLogsAsync(string userId, int pageNr, int pageSize);
    Task<NutritionLog?> UpdateNutritionLogAsync(NutritionLog nutritionLogDTO, int logId);
    Task<NutritionLog?> DeleteNutritionLogAsync(int logId);
    Task<NutritionLog?> CreateNutritionLogAsync(NutritionLog nutritionLog);
    Task<NutritionLog?> GetNutritionLogByIdAsync(int logId);
    Task<ICollection<NutritionLog?>> GetPageAsync(int pageNr, int pageSize);
}
