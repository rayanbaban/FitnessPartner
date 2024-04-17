using FitnessPartner.Models.DTOs;

namespace FitnessPartner.Services.Interfaces
{
    public interface INutritionResourcesService
    {
        Task<ICollection<NutritionResourcesDTO>> GetMyNutritionResourceAsync(int pageNr, int pageSize);
        Task<NutritionResourcesDTO?> UpdateNutritionResourceAsync(NutritionResourcesDTO nutritionResourcesDTO, int resourceId, int loggedinUser);
        Task<NutritionResourcesDTO?> DeleteNutritionResourceAsync(int userId, int resourcesId);
        Task<NutritionResourcesDTO?> CreateNutritionResourceAsync(NutritionResourcesDTO nutritionResources, int loggedinUser);
        Task<NutritionResourcesDTO?> GetNutritionResourceByIdAsync(int resourcesId);
        Task<ICollection<NutritionResourcesDTO>> GetPageAsync(int pageNr, int pageSize);
    }
}
