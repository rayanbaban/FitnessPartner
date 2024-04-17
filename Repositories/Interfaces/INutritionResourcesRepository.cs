using FitnessPartner.Models.Entities;

namespace FitnessPartner.Repositories.Interfaces
{
    public interface INutritionResourcesRepository
    {
        Task<ICollection<NutritionResources>> GetNutritionResourcesAsync(int pageNr, int pageSize);
        Task<NutritionResources?> UpdateNutritionResourceAsync(NutritionResources nutritionResourceDTO, int resourceId, int loggedinUser);
        Task<NutritionResources?> DeleteNutritionResourceAsync(int userId, int resourceId);
        Task<NutritionResources?> CreateNutritionResourceAsync(NutritionResources nutritionResource, int loggedinUser);
        Task<NutritionResources?> GetNutritionResourceByIdAsync(int resourceId);
        Task<ICollection<NutritionResources?>> GetPageAsync(int pageNr, int pageSize);
    }
}
