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

/*[I går 15:32] Rayan Baban
NutritionResourcesTable:
Lagrer artikler og guider om ernæring og kosthold tilpasset ulike treningsmål.
ResourceId (Primærnøkkel)
ResourceTitle
ResourceType (f.eks., artikkel, guide)
Content 

[I går 15:32] Rayan Baban
lage DTO, entity og mapper av dennne. (se hvordan de andre har blitt gjort og herm)*/
