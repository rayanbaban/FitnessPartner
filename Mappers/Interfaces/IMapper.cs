﻿namespace FitnessPartner.Mappers.Interfaces
{
	public interface IMapper<TModel, TDto>
	{
		TDto MapToDto(TModel model);

		TModel MapToModel(TDto dto);
	}
}
