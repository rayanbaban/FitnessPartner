using FitnessPartner.Mappers.Interfaces;
using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Mappers
{
	public class NutritionPlansMapper : IMapper<NutritionPlans, NutritionPlansDTO>
	{
		public NutritionPlansDTO MapToDto(NutritionPlans model)
		{
			return new NutritionPlansDTO(
				model.PlanId,
				model.UserId,
				model.PlanType,
				model.PlanDetails);
		}

		public NutritionPlans MapToModel(NutritionPlansDTO dto)
		{
			return new NutritionPlans()
			{
				PlanId = dto.PlanId,
				PlanDetails = dto.PlanDetails,
				PlanType = dto.PlanType
			};
		}
	}
}
