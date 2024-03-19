using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Mappers
{
    public class NutritionResourcesMapper : IMapper<NutritionResources, NutritionResourcesDTO>
    {
        public NutritionResourcesDTO MapToDto(NutritionResources model)
        {
            return new NutritionResourcesDTO(model.ResourceId, model.ResourceTitle, model.ResourceType, model.Content);
        }

        public NutritionResources MapToModel(NutritionResourcesDTO dto)
        {
            return new NutritionResources
            {
                ResourceId = dto.ResourceId,
                ResourceTitle = dto.ResourceTitle,
                ResourceType = dto.ResourceType,
                Content = dto.Content
            };
        }
    }
}
