using FitnessPartner.Models.DTOs;
using FitnessPartner.Models.Entities;

namespace FitnessPartner.Services.Interfaces
{
	public interface IFitnessGoalsService
	{

		Task<ICollection<FitnessGoalsDTO?>> GetMyFitnessGoalsAsync(string userId, int pageNr, int pageSize);
		Task<ICollection<FitnessGoalsDTO?>> GetAllFitnessGoalsAsync(int pageNr, int pageSize);
		Task<FitnessGoalsDTO?> UpdateFitnessGoalAsync(FitnessGoalsDTO fitnessGoalDTO, int goalId);
		Task<FitnessGoalsDTO?> DeleteFitnessGoalAsync(int goalId);
		Task<FitnessGoalsDTO?> CreateFitnessGoalAsync(FitnessGoalsDTO fitnessGoals);
		Task<FitnessGoalsDTO?> GetFitnessGoalByIdAsync(int goalId);
		Task<ICollection<FitnessGoalsDTO?>> GetPageAsync(int pageNr, int pageSize);

	}
}
