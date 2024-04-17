using FitnessPartner.Models.DTOs;

namespace FitnessPartner.Services.Interfaces
{
    public interface INutritionLogService
    {
        Task<ICollection<NutritionLogDTO>> GetMyNutritionLogsAsync(int pageNr, int pageSize);
        Task<NutritionLogDTO?> UpdateNutritionLogAsync(NutritionLogDTO nutritionLogDTO, int logId, int loggedinUser);
        Task<NutritionLogDTO?> DeleteNutritionLogAsync(int userId, int logId);
        Task<NutritionLogDTO?> CreateNutritionLogAsync(NutritionLogDTO nutritionLog, int loggedinUser);
        Task<NutritionLogDTO?> GetNutritionLogByIdAsync(int logId);
        Task<ICollection<NutritionLogDTO>> GetPageAsync(int pageNr, int pageSize);
    }
}
