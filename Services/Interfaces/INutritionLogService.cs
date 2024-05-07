using FitnessPartner.Models.DTOs;

namespace FitnessPartner.Services.Interfaces
{
    public interface INutritionLogService
    {
        Task<ICollection<NutritionLogDTO>> GetAllNutritionLogsAsync(int pageNr, int pageSize);
        Task<ICollection<NutritionLogDTO>> GetMyNutritionLogsAsync(string userId, int pageNr, int pageSize);
        Task<NutritionLogDTO?> UpdateNutritionLogAsync(NutritionLogDTO nutritionLogDTO, int logId);
        Task<NutritionLogDTO?> DeleteNutritionLogAsync(int logId);
        Task<NutritionLogDTO?> CreateNutritionLogAsync(NutritionLogDTO nutritionLog);
        Task<NutritionLogDTO?> GetNutritionLogByIdAsync(int logId);
        Task<ICollection<NutritionLogDTO>> GetPageAsync(int pageNr, int pageSize);
    }
}
