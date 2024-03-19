using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Mappers
{
    public class NutritionLogMapper : IMapper<NutritionLog, NutritionLogDTO>
    {
        public NutritionLogDTO MapToDto(NutritionLog model)
        {
            return new NutritionLogDTO(model.LogId, model.UserId, model.Date, model.FoodIntake);
        }

        public NutritionLog MapToModel(NutritionLogDTO dto)
        {
            return new NutritionLog
            {
                LogId = dto.LogId,
                UserId = dto.UserId,
                Date = dto.Date,
                FoodIntake = dto.FoodIntake,

            };
        }
    }
}
