using FitnessPartner.Models.DTOs;

namespace FitnessPartner.Services.Interfaces
{
    public interface INutritionLogService
    {
        Task<ICollection<NutritionLogDTO>> GetMyNutritionLogsAsync(int pageNr, int pageSize);
        Task<NutritionLogDTO?> UpdateNutritionLogAsync(NutritionLogDTO nutritionLogDTO, int logId);
        Task<NutritionLogDTO?> DeleteNutritionLogAsync(int logId);
        Task<NutritionLogDTO?> CreateNutritionLogAsync(NutritionLogDTO nutritionLog);
        Task<NutritionLogDTO?> GetNutritionLogByIdAsync(int logId);
        Task<ICollection<NutritionLogDTO>> GetPageAsync(int pageNr, int pageSize);
    }
}
