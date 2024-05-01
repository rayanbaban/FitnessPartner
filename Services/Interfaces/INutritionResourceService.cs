using FitnessPartner.Models.DTOs;

namespace FitnessPartner.Services.Interfaces
{
    public interface INutritionResourceService
    {
        Task<ICollection<NutritionResourcesDTO>> GetMyNutritionResourceAsync(int pageNr, int pageSize);
        Task<NutritionResourcesDTO?> UpdateNutritionResourceAsync(NutritionResourcesDTO nutritionResourcesDTO, int resourceId);
        Task<NutritionResourcesDTO?> DeleteNutritionResourceAsync(int resourcesId);
        Task<NutritionResourcesDTO?> CreateNutritionResourceAsync(NutritionResourcesDTO nutritionResources);
        Task<NutritionResourcesDTO?> GetNutritionResourceByIdAsync(int resourcesId);
        Task<ICollection<NutritionResourcesDTO>> GetPageAsync(int pageNr, int pageSize);
    }
}
